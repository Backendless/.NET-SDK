// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Weborb.ProxyGen.DynamicProxy.Generators.Emitters
{
	using System;
	using System.Collections;
    using System.Collections.Generic;
	using System.Reflection.Emit;

	public sealed class LdcOpCodesDictionary : Dictionary<Type, OpCode>
	{
		private static readonly LdcOpCodesDictionary _dict = new LdcOpCodesDictionary();

		private static readonly OpCode _emptyOpCode = new OpCode();

		private LdcOpCodesDictionary() : base()
		{
			this[typeof(bool)] = OpCodes.Ldc_I4;
            this[ typeof( char ) ] = OpCodes.Ldc_I4;
            this[ typeof( SByte ) ] = OpCodes.Ldc_I4;
            this[ typeof( Int16 ) ] = OpCodes.Ldc_I4;
            this[ typeof( Int32 ) ] = OpCodes.Ldc_I4;
            this[ typeof( Int64 ) ] = OpCodes.Ldc_I8;
            this[ typeof( float ) ] = OpCodes.Ldc_R4;
            this[ typeof( double ) ] = OpCodes.Ldc_R8;
            this[ typeof( byte ) ] = OpCodes.Ldc_I4_0;
            this[ typeof( UInt16 ) ] = OpCodes.Ldc_I4_0;
            this[ typeof( UInt32 ) ] = OpCodes.Ldc_I4_0;
            this[ typeof( UInt64 ) ] = OpCodes.Ldc_I4_0;
		}


		public static LdcOpCodesDictionary Instance
		{
			get { return _dict; }
		}

		public static OpCode EmptyOpCode
		{
			get { return _emptyOpCode; }
		}
	}
}