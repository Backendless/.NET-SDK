using System;
using System.IO;
using System.Globalization;
using System.Text;

namespace Weborb.Protocols.JsonRPC
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only means of 
    /// emitting JSON data formatted as JSON text (RFC 4627).
    /// </summary>

    public class JsonTextWriter : JsonWriter
    {
        private readonly TextWriter _writer;

        //
        // Pretty printing as per:
        // http://developer.mozilla.org/es4/proposals/json_encoding_and_decoding.html
        //
        // <quote>
        // ...linefeeds are inserted after each { and , and before } , and multiples 
        // of 4 spaces are inserted to indicate the level of nesting, and one space 
        // will be inserted after :. Otherwise, no whitespace is inserted between 
        // the tokens.
        // </quote>
        //
        
        private bool _prettyPrint;
        private bool _newLine;
        private int _indent;
        private char[] _indentBuffer;

        public JsonTextWriter() :
            this(null) {}

        public JsonTextWriter(TextWriter writer)
        {
            _writer = writer != null ? writer : new StringWriter();
        }

        public bool PrettyPrint
        {
            get { return _prettyPrint; }
            set { _prettyPrint = value; }
        }

        protected TextWriter InnerWriter
        {
            get { return _writer; }
        }

        public override void Flush()
        {
            _writer.Flush();
        }

        public override string ToString()
        {
            StringWriter stringWriter = _writer as StringWriter;
            return stringWriter != null ? 
                stringWriter.ToString() : base.ToString();
        }

        protected override void WriteStartObjectImpl()
        {
            OnWritingValue();
            WriteDelimiter('{');
            PrettySpace();
        }
        
        protected override void WriteEndObjectImpl()
        {
            if (Index > 0)
            {
                PrettyLine();
                _indent--;
            }

            WriteDelimiter('}');
        }

        protected override void WriteMemberImpl(string name)
        {
            if (Index > 0)
            {
                WriteDelimiter(',');
                PrettyLine();
            }
            else
            {
                PrettyLine();
                _indent++;
            }
            
            WriteStringImpl(name);
            WriteDelimiter(':');
            PrettySpace();
        }

        public void WriteCachedJSON( string p )
          {
          EnsureMemberOnObjectBracket();
          WriteScalar( p );
          OnValueWritten();
          }

        protected override void WriteStringImpl(string value)
        {
            WriteScalar(Enquote(value, null).ToString());
        }

        private static StringBuilder Enquote( string s, StringBuilder sb )
          {
          if ( s == null || s.Length == 0 )
            return new StringBuilder( "\"\"" );


          int length = (s == null ? "" : s).Length;

          if ( sb == null )
            sb = new StringBuilder( length + 4 );

          sb.Append( '"' );

          char last;
          char ch = '\0';

          for ( int index = 0; index < length; index++ )
            {
            last = ch;
            ch = s[ index ];

            switch ( ch )
              {
              case '\\':
              case '"':
                  {
                  sb.Append( '\\' );
                  sb.Append( ch );
                  break;
                  }

              case '/':
                    {
                    if ( last == '<' )
                      sb.Append( '\\' );
                    sb.Append( ch );
                    break;
                    }

              case '\b': sb.Append( "\\b" ); break;
              case '\t': sb.Append( "\\t" ); break;
              case '\n': sb.Append( "\\n" ); break;
              case '\f': sb.Append( "\\f" ); break;
              case '\r': sb.Append( "\\r" ); break;

              default:
                  {
                  if ( ch < ' ' )
                    {
                    sb.Append( "\\u" );
                    sb.Append( ( (int)ch ).ToString( "x4", CultureInfo.InvariantCulture ) );
                    }
                  else
                    {
                    sb.Append( ch );
                    }

                  break;
                  }
              }
            }

          return sb.Append( '"' );
          }

        protected override void WriteNumberImpl(string value)
        {
            WriteScalar(value);
        }

        protected override void WriteBooleanImpl(bool value)
        {
            WriteScalar(value ? "true" : "false");
        }

        protected override void WriteNullImpl()
        {
            WriteScalar( "null" );
        }

        protected override void WriteStartArrayImpl()
        {
            OnWritingValue();
            WriteDelimiter('[');
            PrettySpace();
        }

        protected override void WriteEndArrayImpl()
        {
            if (IsNonEmptyArray())
                PrettySpace();

            WriteDelimiter(']');
        }

        public void WriteScalar(string text)
        {
            OnWritingValue();
            PrettyIndent();
            _writer.Write(text);
        }
        
        private bool IsNonEmptyArray()
        {
            return Bracket == JsonWriterBracket.Array && Index > 0;
        }
        
        //
        // Methods below are mostly related to pretty-printing of JSON text.
        //

        private void OnWritingValue()
        {
            if (IsNonEmptyArray())
            {
                WriteDelimiter(',');
                PrettySpace();
            }
        }

        private void WriteDelimiter(char ch)
        {
            PrettyIndent();
            _writer.Write(ch);
        }

        private void PrettySpace()
        {
            if (!_prettyPrint) return;
            WriteDelimiter(' ');
        }

        private void PrettyLine()
        {
            if (!_prettyPrint) return;
            _writer.WriteLine();
            _newLine = true;
        }

        private void PrettyIndent() 
        {
            if (!_prettyPrint)
                return;
            
            if (_newLine)
            {
                if (_indent > 0)
                {
                    int spaces = _indent * 4;
                    
                    if (_indentBuffer == null || _indentBuffer.Length < spaces)
                        _indentBuffer = new string(' ', spaces * 4).ToCharArray();
                    
                    _writer.Write(_indentBuffer, 0, spaces);
                }
                
                _newLine = false;
            }
        }
    }
}
