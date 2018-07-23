using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using Weborb.Protocols.JsonRPC;

namespace Weborb.Protocols.JsonRPC
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only means of 
    /// emitting JSON data.
    /// </summary>

    public abstract class JsonWriter : IDisposable
    {
        private WriterStateStack _stateStack;
        private WriterState _state;
        private int _maxDepth = 100000;

        protected JsonWriter()
        {
            _state = new WriterState(JsonWriterBracket.Pending);
        }
        
        public int Depth
        {
            get { return HasStates ? States.Count : 0; }
        }

        public int MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        public int Index
        {
            get { return Depth == 0 ? -1 : _state.Index; }
        }
        
        public JsonWriterBracket Bracket
        {
            get { return _state.Bracket; }
        }

        public void WriteStartObject()
        {
            EnteringBracket();
            WriteStartObjectImpl();
            EnterBracket(JsonWriterBracket.Object);
        }

        public void WriteEndObject()
        {
            if (_state.Bracket != JsonWriterBracket.Object)
                throw new Exception("JSON Object tail not expected at this time.");
            
            WriteEndObjectImpl();
            ExitBracket();
        }

        public void WriteMember(string name)
        {
            if (_state.Bracket != JsonWriterBracket.Object)
                throw new Exception("A JSON Object member is not valid inside a JSON Array.");

            WriteMemberImpl(name);
            _state.Bracket = JsonWriterBracket.Member;
        }

        public void WriteStartArray()
        {
            EnteringBracket();
            WriteStartArrayImpl();
            EnterBracket(JsonWriterBracket.Array);
        }

        public void WriteEndArray()
        {
            if (_state.Bracket != JsonWriterBracket.Array)
                throw new Exception("JSON Array tail not expected at this time.");
            
            WriteEndArrayImpl();
            ExitBracket();
        }

        public void WriteString(string value)
        {
            if (Depth == 0)
            {
                WriteStartArray(); WriteString(value); WriteEndArray();
            }
            else
            {
                EnsureMemberOnObjectBracket();
                WriteStringImpl(value);
                OnValueWritten();
            }
        }

        public void WriteNumber(string value)
        {
            if (Depth == 0)
            {
                WriteStartArray(); WriteNumber(value); WriteEndArray();
            }
            else
            {
                EnsureMemberOnObjectBracket();
                WriteNumberImpl(value);
                OnValueWritten();
            }
        }

        public void WriteBoolean(bool value)
        {
            if (Depth == 0)
            {
                WriteStartArray(); WriteBoolean(value); WriteEndArray();
            }
            else
            {
                EnsureMemberOnObjectBracket();
                WriteBooleanImpl(value);
                OnValueWritten();
            }
        }

        public void WriteNull()
        {
            if (Depth == 0)
            {
                WriteStartArray(); WriteNull(); WriteEndArray();
            }
            else 
            {
                EnsureMemberOnObjectBracket();
                WriteNullImpl();
                OnValueWritten();
            }
        }
        
        //
        // Actual methods that need to be implemented by the subclass.
        // These methods do not need to check for the structural 
        // integrity since this is checked by this base implementation.
        //
        
        protected abstract void WriteStartObjectImpl();
        protected abstract void WriteEndObjectImpl();
        protected abstract void WriteMemberImpl(string name);
        protected abstract void WriteStartArrayImpl();
        protected abstract void WriteEndArrayImpl();
        protected abstract void WriteStringImpl(string value);
        protected abstract void WriteNumberImpl(string value);
        protected abstract void WriteBooleanImpl(bool value);
        protected abstract void WriteNullImpl();

        private bool HasStates
        {
            get { return _stateStack != null && _stateStack.Count > 0; }
        }

        private WriterStateStack States
        {
            get
            {
                if (_stateStack == null)
                    _stateStack = new WriterStateStack();
                
                return _stateStack;
            }
        }

        private void EnteringBracket()
        {
            EnsureNotEnded();

            if (_state.Bracket != JsonWriterBracket.Pending)
                EnsureMemberOnObjectBracket();

            if (Depth + 1 > MaxDepth)
                throw new Exception("Maximum allowed depth has been exceeded.");
        }

        private void EnterBracket(JsonWriterBracket newBracket)
        {
            Debug.Assert(newBracket == JsonWriterBracket.Array || newBracket == JsonWriterBracket.Object);
            
            States.Push(_state);
            _state = new WriterState(newBracket);
        }
        
        private void ExitBracket()
        {
            _state = States.Pop();

            if (_state.Bracket == JsonWriterBracket.Pending)
                _state.Bracket = JsonWriterBracket.Closed;
            else            
                OnValueWritten();
        }

        protected void OnValueWritten()
        {
            if (_state.Bracket == JsonWriterBracket.Member) 
                _state.Bracket = JsonWriterBracket.Object;
            
            _state.Index++;
        }

        protected void EnsureMemberOnObjectBracket() 
        {
            if (_state.Bracket == JsonWriterBracket.Object)
                throw new Exception("A JSON member value inside a JSON object must be preceded by its member name.");
        }

        private void EnsureNotEnded()
        {
            if (_state.Bracket == JsonWriterBracket.Closed)
                throw new Exception("JSON data has already been ended.");
        }

        [ Serializable ]
        private struct WriterState
        {
            public JsonWriterBracket Bracket;
            public int Index;

            public WriterState(JsonWriterBracket bracket)
            {
                Bracket = bracket;
                Index = 0;
            }
        }
        
        [ Serializable ]
        private sealed class WriterStateStack
        {
            private WriterState[] _states;
            private int _count;

            public int Count
            {
                get { return _count; }
            }

            public void Push(WriterState state)
            {
                if (_states == null)
                {
                    _states = new WriterState[6];
                }
                else if (_count == _states.Length)
                {
                    WriterState[] items = new WriterState[_states.Length * 2];
                    _states.CopyTo(items, 0);
                    _states = items;
                }
                
                _states[_count++] = state;
            }
            
            public WriterState Pop()
            {
                if (_count == 0)
                    throw new InvalidOperationException();
                
                WriterState state = _states[--_count];
                
                if (_count == 0)
                    _states = null;
                
                return state;
            }
        }
       
        /// <summary>
        /// When overridden in a derived class, flushes whatever is in the 
        /// buffer to the underlying streams and also flushes the 
        /// underlying stream. The default implementation does nothing.
        /// </summary>
        
        public virtual void Flush() {}

        /// <summary>
        /// Closes the writer and releases any underlying resources 
        /// associated with the writer.
        /// </summary>
        
        public virtual void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        

        /// <summary>
        /// Writes a JSON number from an <see cref="Int32"/> value.
        /// </summary>

        public void WriteNumber(int value)
        {
            WriteNumber(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Writes a JSON number from a <see cref="Double"/> value.
        /// </summary>

        public void WriteNumber(double value)
        {
            if (double.IsNaN(value))
                throw new ArgumentOutOfRangeException("value");

            WriteNumber(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Writes a JSON array of JSON strings given an enumerable source
        /// of arbitrary <see cref="Object"/> values.
        /// </summary>

        public void WriteStringArray(IEnumerable values)
        {
            if (values == null)
            {
                WriteNull();
            }
            else
            {
                WriteStartArray();
                        
                foreach (object value in values)
                {
                    if (LogicallyEquals(value))
                        WriteNull();
                    else
                        WriteString(value.ToString());
                }
                        
                WriteEndArray();
            }
        }

        public static bool LogicallyEquals( object o )
          {
          //
          // Equals a null reference?
          //

          if ( o == null )
            return true;

          //
          // Equals the logical null value used in database applications?
          //

          if ( Convert.IsDBNull( o ) )
            return true;

          //
          // Instance is not one of the known logical null values.
          //

          return false;
          }

        /// <summary>
        /// Writes a JSON array of JSON strings given an array of 
        /// <see cref="String"/> values.
        /// </summary>

        public void WriteStringArray(params string[] values)
        {
            if (values == null)
            {
                WriteNull();
            }
            else
            {
                WriteStartArray();
                        
                foreach (string value in values)
                {
                    if (LogicallyEquals(value))
                        WriteNull();
                    else
                        WriteString(value);
                }
                        
                WriteEndArray();
            }
        }

        /// <summary>
        /// Writes the next value from the given <see cref="JsonReader"/>
        /// into this writer's output. If the reader is positioned
        /// at the root of JSON data, then the entire data will be
        /// written.
        /// </summary>

        public virtual void WriteFromReader(JsonReader reader)
        {
            if (reader == null)            
                throw new ArgumentNullException("reader");

            if (!reader.MoveToContent())
                return;

            if (reader.TokenClass == JsonTokenClass.String)
            {
                WriteString(reader.Text); 
            }
            else if (reader.TokenClass == JsonTokenClass.Number)
            {
                WriteNumber(reader.Text);
            }
            else if (reader.TokenClass == JsonTokenClass.Boolean)
            {
                WriteBoolean(reader.Text == "true"); 
            }
            else if (reader.TokenClass == JsonTokenClass.Null)
            {
                WriteNull();
            }
            else if (reader.TokenClass == JsonTokenClass.Array)
            {
                WriteStartArray();
                reader.Read();

                while (reader.TokenClass != JsonTokenClass.EndArray)
                    WriteFromReader(reader);

                WriteEndArray();
            }
            else if (reader.TokenClass == JsonTokenClass.Object)
            {
                reader.Read();
                WriteStartObject();
                    
                while (reader.TokenClass != JsonTokenClass.EndObject)
                {
                    WriteMember(reader.ReadMember());
                    WriteFromReader(reader);
                }

                WriteEndObject();
            }
            else 
            {
                throw new Exception(string.Format("{0} not expected.", reader.TokenClass));
            }

            reader.Read();
        }
        
        public void AutoComplete()
        {
            if (Depth == 0)
                throw new InvalidOperationException();
            
            if (Bracket == JsonWriterBracket.Member)
                WriteNull();
            
            while (Depth > 0)
            {
                if (Bracket == JsonWriterBracket.Object)
                    WriteEndObject();
                else if (Bracket == JsonWriterBracket.Array)
                    WriteEndArray();
                else 
                    throw new Exception("Implementation error.");
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
