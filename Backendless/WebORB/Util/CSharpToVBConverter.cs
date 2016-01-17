using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCodeInternal.NRefactory;
using System.IO;
using ICSharpCodeInternal.NRefactory.PrettyPrinter;
using ICSharpCodeInternal.NRefactory.Visitors;

namespace Weborb.Util
{
    public static class CSharpToVBConverter
    {
        public static String Convert(String csharpCode)
        {
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, 
                    new StringReader(csharpCode));

            parser.Parse();
            parser.CompilationUnit.AcceptVisitor(new CSharpToVBNetConvertVisitor(), null);

            VBNetOutputVisitor outputVisitor = new VBNetOutputVisitor();
            outputVisitor.VisitCompilationUnit(parser.CompilationUnit, null);

            return outputVisitor.Text; 
        }
    }
}
