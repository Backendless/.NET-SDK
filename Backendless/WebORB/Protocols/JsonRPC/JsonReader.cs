using System;

namespace Weborb.Protocols.JsonRPC
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only 
    /// access to JSON data. 
    /// </summary>

    public abstract class JsonReader : IDisposable
    {
        private JsonToken _token;
        private int _depth;
        private int _maxDepth = 100000;

        protected JsonReader()
        {
            _token = JsonToken.BOF();
        }

        /// <summary>
        /// Gets the current token.
        /// </summary>

        public JsonToken Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Return the current level of nesting as the reader encounters
        /// nested objects and arrays.
        /// </summary>

        public int Depth
        {
            get { return _depth; }
        }

        /// <summary>
        /// Sets or returns the maximum allowed depth or level of nestings
        /// of objects and arrays.
        /// </summary>

        public int MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        /// <summary>
        /// Reads the next token and returns true if one was found.
        /// </summary>

        public bool Read()
        {
            if (!EOF)
            {
                if (Depth > MaxDepth)
                    throw new Exception("Maximum allowed depth has been exceeded.");

                if (TokenClass == JsonTokenClass.EndObject || TokenClass == JsonTokenClass.EndArray)
                    _depth--;

                _token = ReadTokenImpl();

                if (TokenClass == JsonTokenClass.Object || TokenClass == JsonTokenClass.Array)
                    _depth++;
            }
            
            return !EOF;
        }

        /// <summary>
        /// Reads the next token and returns it.
        /// </summary>
        
        protected abstract JsonToken ReadTokenImpl();


        /// <summary>
        /// Gets the class of the current token.
        /// </summary>

        public JsonTokenClass TokenClass
        {
            get { return Token.Class; }
        }

        /// <summary>
        /// Gets the text of the current token.
        /// </summary>

        public string Text
        {
            get { return Token.Text; }
        }

        /// <summary>
        /// Closes the reader and releases any underlying resources 
        /// associated with the reader.
        /// </summary>
        
        public virtual void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the state of the 
        /// instance.
        /// </summary>

        public override string ToString()
        {
            return Token.ToString();
        }
        
        /// <summary>
        /// Indicates whether the reader has reached the end of input source.
        /// </summary>

        public bool EOF
        {
            get { return TokenClass == JsonTokenClass.EOF; }
        }

        /// <summary>
        /// Reads the next token ensuring that it matches the specified 
        /// token. If not, an exception is thrown.
        /// </summary>

        public string ReadToken(JsonTokenClass token)
        {
            int depth = Depth;
            
            if (!token.IsTerminator)
                MoveToContent();
            
            //
            // We allow an exception to the simple case of validating
            // the token and returning its value. If the reader is still at
            // the start (depth is zero) and we're being asked to check
            // for the null token or a scalar-type token then we allow that 
            // to be appear within a one-length array. This is done because
            // valid JSON text must begin with an array or object. Our
            // JsonWriterBase automatically wraps a scalar value in an 
            // array if not done explicitly. This exception here allow
            // that case to pass as being logically valid, as if the
            // token appeared entirely on its own between BOF and EOF.
            //
            
            string text;

            if (depth == 0 && TokenClass == JsonTokenClass.Array &&
                (token.IsScalar || token == JsonTokenClass.Null))
            {
                Read(/* array */);
                text = ReadToken(token);
                ReadToken(JsonTokenClass.EndArray);
            }
            else
            {
                if (TokenClass != token)
                    throw new Exception(string.Format("Found {0} where {1} was expected.", TokenClass, token));
            
                text = Text;
                Read();
            }
            
            return text;
        }

        /// <summary>
        /// Reads the next token, ensures it is a String and returns its 
        /// text. If the next token is not a String, then an exception
        /// is thrown instead.
        /// </summary>

        public string ReadString()
        {
            return ReadToken(JsonTokenClass.String);
        }

        /// <summary>
        /// Reads the next token, ensures it is a Null. If the next token 
        /// is not a Null, then an exception is thrown instead.
        /// </summary>
        
        public void ReadNull()
        {
            ReadToken(JsonTokenClass.Null);
        }

        /// <summary>
        /// Reads the next token, ensures it is Member (of an object) and 
        /// returns its text. If the next token is not a Member, then an 
        /// exception is thrown instead.
        /// </summary>

        public string ReadMember()
        {
            return ReadToken(JsonTokenClass.Member);
        }
        
        /// <summary>
        /// Steps out of the current depth to the level immediately above. 
        /// Usually this skips the current Object or Array being read, 
        /// including all nested structures.
        /// </summary>
        
        public void StepOut()
        {
            int depth = Depth;
            
            if (depth == 0)
                throw new InvalidOperationException();

            while (Depth > depth || (TokenClass != JsonTokenClass.EndObject && TokenClass != JsonTokenClass.EndArray))
                Read();
            
            Read(/* past tail */);
        }

        /// <summary>
        /// Skips through the next item. If it is an Object or Array, then
        /// the entire object or array is skipped. If it is a scalar value
        /// then just that value is skipped. If the reader is on an object
        /// member then the member and its value are skipped.
        /// </summary>

        public void SkipItem()
        {
            if (!MoveToContent())
                return;
            
            if (TokenClass == JsonTokenClass.Object || TokenClass == JsonTokenClass.Array)
            {
                StepOut();
            }
            else if (TokenClass == JsonTokenClass.Member)
            {
                Read(/* member */);
                SkipItem(/* value */);
            }
            else
            {
                Read(/* scalar */);
            }
        }

        /// <summary>
        /// Ensures that the reader is positioned on content (a JSON value) 
        /// ready to be read. If the reader is already aligned on the start
        /// of a value then no further action is taken.
        /// </summary>
        /// <returns>Return true if content was found. Otherwise false to 
        /// indicate EOF.</returns>

        public bool MoveToContent()
        {
            if (!EOF)
            {
                if (TokenClass.IsTerminator)
                    return Read();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Represents the method that handles the Disposed event of a reader.
        /// </summary>
        
        public virtual event EventHandler Disposed;
        
        void IDisposable.Dispose()
        {
            Close();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                OnDisposed(EventArgs.Empty);
        }

        private void OnDisposed(EventArgs e)
        {
            EventHandler handler = Disposed;
            
            if (handler != null)
                handler(this, e);
        }
    }
}
