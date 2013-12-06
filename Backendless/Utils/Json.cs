using System.Collections.Generic;

namespace BackendlessAPI.Utils
{
    public class Json
    {
        #region Fields & Constants

        private const string INVALID_JSON_MSG = "Invalid json";

        private enum JsonTypes
        {
            KEY_VALUE,
            KEY_ARRAY,
            KEY_OBJECT
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parses json string and returns a dictionary with kay value pairs
        /// </summary>
        /// <param name="json">Json string to be parsed</param>
        /// <returns>Dictionary containing all key value pairs</returns>
        public Dictionary<string, object> Deserialize(string json)
        {
            #region Validation

            if (json == null)
                ThrowError(INVALID_JSON_MSG);

            #endregion

            Dictionary<string, object> dict = new Dictionary<string, object>();

            json = json.Trim();

            #region Validation

            if (json.Length == 0 || json[0] != '{')
                ThrowError(INVALID_JSON_MSG);

            #endregion

            json = json.Remove(0, 1); //Removing first {

            while (json.Trim() != string.Empty)
            {
                JsonTypes nextDataType = GetNextDataType(json);

                switch (nextDataType)
                {
                    case JsonTypes.KEY_VALUE:
                        SetKeyValue(ref json, dict);
                        break;
                    case JsonTypes.KEY_ARRAY:
                        SetKeyArray(ref json, dict);
                        break;
                    case JsonTypes.KEY_OBJECT:
                        SetKeyObject(ref json, dict);
                        break;
                }
            }

            return dict;
        }

        /// <summary>
        /// Gets the type of data present in the next sequence
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private JsonTypes GetNextDataType(string data)
        {
            JsonTypes type = JsonTypes.KEY_VALUE;

            string tempData = data;
            int colonIndex = IndexOfJsonSpecialChar(data, ':', '\0');

            if (colonIndex == -1)
                ThrowError(INVALID_JSON_MSG);

            tempData = tempData.Remove(0, colonIndex + 1).Trim();

            if (tempData.Length > 1)
            {
                char valueFirstChar = tempData[0];

                if (valueFirstChar == '{')
                    type = JsonTypes.KEY_OBJECT;
                else if (valueFirstChar == '[')
                    type = JsonTypes.KEY_ARRAY;
                else
                    type = JsonTypes.KEY_VALUE;
            }
            else
                ThrowError(INVALID_JSON_MSG);

            return type;
        }

        /// <summary>
        /// Gets the index of special characters used in json representation. Occurence of these characters inside a string is ignored.
        /// </summary>
        /// <param name="data">Json data</param>
        /// <param name="splChar1">The primary special character to be searched</param>
        /// <param name="splChar2">Secondary special character to be searched. This is optional.</param>
        /// <returns>Index of the first occurence of the special character</returns>
        private int IndexOfJsonSpecialChar(string data, char splChar1, char splChar2)
        {
            int splCharIndex = -1;
            int index = 0;
            Stack<char> doubleQuotes = new Stack<char>();

            while (index < data.Length)
            {
                if ((data[index] == splChar1 || data[index] == splChar2) && doubleQuotes.Count == 0)
                    return index;

                else if (data[index] == '"')
                {
                    if (doubleQuotes.Count == 0)
                        doubleQuotes.Push('"');
                    else
                        doubleQuotes.Pop();
                }

                index++;
            }


            return splCharIndex;
        }

        /// <summary>
        /// Sets key and value pair in the dictionary
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dict"></param>
        private void SetKeyValue(ref string data, Dictionary<string, object> dict)
        {
            string key;
            string val;

            try
            {
                int endIndex = IndexOfJsonSpecialChar(data, ',', '}');

                string keyVal = data.Substring(0, endIndex);
                keyVal = keyVal.Trim();

                int colonIndex = IndexOfJsonSpecialChar(keyVal, ':', '\0');

                //Key
                key = keyVal.Substring(0, colonIndex).Trim();
                key = key.Substring(1, key.Length - 2); //Removing double quotes

                //Value
                val = keyVal.Remove(0, colonIndex + 1).Trim(); //Removing key & colon

                if ( val == null || val == string.Empty || val.Equals( "null" ) ) //value is null
                    dict.Add(key, null);
                else if (val[0] == '"') //Value is string
                    dict.Add(key, val.Substring(1, val.Length - 2)); //Leaving double quotes in the value and adding
                else
                {
                    if (val.ToLower() == "true" || val.ToLower() == "false") //Check for boolean type
                        dict.Add(key, bool.Parse(val.ToLower()));
                    else //number type
                    {
                        try
                        {
                            if (val.Contains(".")) //Double type
                                dict.Add(key, double.Parse(val));
                            else //Integer type
                                dict.Add(key, int.Parse(val));
                        }
                        catch (System.Exception)
                        {
                            ThrowError(INVALID_JSON_MSG);
                        }
                    }
                }

                data = data.Remove(0, endIndex + 1);
            }
            catch (System.Exception)
            {
                ThrowError(INVALID_JSON_MSG);
            }
        }

        /// <summary>
        /// Sets key and object in the dictionary after parsing object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dict"></param>
        private void SetKeyObject(ref string data, Dictionary<string, object> dict)
        {
            string key;
            string val;

            try
            {
                int colonIndex = IndexOfJsonSpecialChar(data, ':', '\0');

                //Key
                key = data.Substring(0, colonIndex).Trim();
                key = key.Substring(1, key.Length - 2); //Leaving double quotes

                data = data.Remove(0, colonIndex + 1);

                //Value
                int valEndIndex = GetEndIndex(data, '{', '}');
                val = data.Substring(0, valEndIndex + 1).Trim();

                dict.Add(key, Deserialize(val)); //Adding key and object to the dictionary

                data = data.Remove(0, valEndIndex + 1);
                data = data.Remove(0, IndexOfJsonSpecialChar(data, ',', '}') + 1);
            }
            catch (System.Exception)
            {
                ThrowError(INVALID_JSON_MSG);
            }
        }

        /// <summary>
        /// Sets key and array in the dictionary after splitting array elements according to their data type
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dict"></param>
        private void SetKeyArray(ref string data, Dictionary<string, object> dict)
        {
            string key;
            string val;

            try
            {
                int colonIndex = IndexOfJsonSpecialChar(data, ':', '\0');

                //Key
                key = data.Substring(0, colonIndex).Trim();
                key = key.Substring(1, key.Length - 2); //Leaving double quotes

                data = data.Remove(0, colonIndex + 1);

                //Value
                int valEndIndex = GetEndIndex(data, '[', ']');
                val = data.Substring(0, valEndIndex + 1).Trim();

                dict.Add(key, GetArrayValues(val));

                data = data.Remove(0, valEndIndex + 1);
                data = data.Remove(0, IndexOfJsonSpecialChar(data, ',', '}') + 1);
            }
            catch (System.Exception)
            {
                ThrowError(INVALID_JSON_MSG);
            }
        }

        /// <summary>
        /// Fetches array values according to a data type of the array values
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private object GetArrayValues(string array)
        {
            array = array.Trim();
            array = array.Remove(0, 1); //Removing first [
            array = array.Remove(array.Length - 1).Trim(); //Removing last ]

            if (array.Length > 0 && array[0] == '{') //Array contents are objects
            {
                List<object> arrayvalues = new List<object>();

                while (array.Trim() != string.Empty)
                {
                    int valEndIndex = GetEndIndex(array, '{', '}');
                    string val = array.Substring(0, valEndIndex + 1).Trim();

                    arrayvalues.Add(Deserialize(val)); //Adding object to the dictionary

                    array = array.Remove(0, valEndIndex + 1);
                    array = array.Remove(0, IndexOfJsonSpecialChar(array, ',', '}') + 1);
                }

                return arrayvalues;
            }
            else //Array contents are not objects
            {
                List<string> arrayvalues = new List<string>();

                int commaIndex = IndexOfJsonSpecialChar(array, ',', '\0');

                while (commaIndex != -1)
                {
                    arrayvalues.Add(array.Substring(0, commaIndex));
                    array = array.Remove(0, commaIndex + 1);
                    commaIndex = IndexOfJsonSpecialChar(array, ',', '\0');
                }

                if (array.Trim() != string.Empty)
                    arrayvalues.Add(array);

                if (arrayvalues.Count == 0) //Null value
                    return null;

                string compareVal = string.Empty;

                //Finding the first non-empty value to set in compareVal variable which is used to identify data type of the array
                foreach (string val in arrayvalues)
                {
                    if (val.Trim() != string.Empty)
                    {
                        compareVal = val.Trim();
                        break;
                    }
                }

                if (compareVal == string.Empty)
                    return null;

                try
                {
                    if (compareVal[0] == '"') //String Arrray
                    {
                        List<string> stringArray = new List<string>();

                        for (int valCnt = 0; valCnt < arrayvalues.Count; valCnt++)
                        {
                            string val = arrayvalues[valCnt].ToString().Trim();
                            stringArray.Add(val.Substring(1, val.Length - 2));
                                //Leaving double quotes in the value and adding
                        }

                        return stringArray;
                    }
                    else if (compareVal.ToLower() == "true" || compareVal.ToLower() == "false") //Boolean array
                    {
                        List<bool> boolArray = new List<bool>();

                        for (int valCnt = 0; valCnt < arrayvalues.Count; valCnt++)
                        {
                            string val = arrayvalues[valCnt].ToString().Trim();
                            boolArray.Add(bool.Parse(val.ToLower()));
                        }

                        return boolArray;
                    }
                    else //Double array
                    {
                        List<double> doubleArray = new List<double>();

                        for (int valCnt = 0; valCnt < arrayvalues.Count; valCnt++)
                        {
                            string val = arrayvalues[valCnt].ToString().Trim();
                            doubleArray.Add(double.Parse(val));
                        }

                        return doubleArray;
                    }
                }
                catch (System.Exception)
                {
                    ThrowError(INVALID_JSON_MSG);
                }
            }

            return null;
        }

        private int GetEndIndex(string data, char open, char close)
        {
            int index = 0;
            Stack<char> stack = new Stack<char>();
            Stack<char> doubleQuotes = new Stack<char>();

            //Adds open char to the stack and pops out the open char from stack when close char occurs.
            //This is repeated until count becomes 0 and the index is returned

            while (index < data.Length)
            {
                if (data[index] == '"')
                {
                    if (doubleQuotes.Count == 0)
                        doubleQuotes.Push('"');
                    else
                        doubleQuotes.Pop();
                }

                else if (data[index] == open && doubleQuotes.Count == 0)
                    stack.Push(open);

                else if (data[index] == close && stack.Count > 0 && doubleQuotes.Count == 0)
                {
                    stack.Pop();

                    if (stack.Count == 0)
                        return index;
                }

                index++;
            }

            return -1;
        }

        private void ThrowError(string msg)
        {
            throw new System.Exception(msg);
        }

        #endregion
    }
}