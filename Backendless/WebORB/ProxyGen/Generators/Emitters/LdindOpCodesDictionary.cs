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

	public sealed class LdindOpCodesDictionary : Dictionary<Type, OpCode>
	{
		private static readonly LdindOpCodesDictionary _dict = new LdindOpCodesDictionary();

		private static readonly OpCode _emptyOpCode = new OpCode();

		private LdindOpCodesDictionary() : base()
		{
			this[typeof(bool)] = OpCodes.Ldind_I1;
            this[ typeof( char ) ] = OpCodes.Ldind_I2;
            this[ typeof( SByte ) ] = OpCodes.Ldind_I1;
            this[ typeof( Int16 ) ] = OpCodes.Ldind_I2;
            this[ typeof( Int32 ) ] = OpCodes.Ldind_I4;
            this[ typeof( Int64 ) ] = OpCodes.Ldind_I8;
            this[ typeof( float ) ] = OpCodes.Ldind_R4;
            this[ typeof( double ) ] = OpCodes.Ldind_R8;
            this[ typeof( byte ) ] = OpCodes.Ldind_U1;
            this[ typeof( UInt16 ) ] = OpCodes.Ldind_U2;
            this[ typeof( UInt32 ) ] = OpCodes.Ldind_U4;
            this[ typeof( UInt64 ) ] = OpCodes.Ldind_I8;
		}

		public static LdindOpCodesDictionary Instance
		{
			get { return _dict; }
		}

		public static OpCode EmptyOpCode
		{
			get { return _emptyOpCode; }
		}
	}
}