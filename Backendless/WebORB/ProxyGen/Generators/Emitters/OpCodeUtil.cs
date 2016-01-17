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
	using System.Reflection.Emit;

	internal abstract class OpCodeUtil
	{
		public static void EmitLoadOpCodeForConstantValue(ILGenerator gen, object value)
		{
			if (value is String)
			{
				gen.Emit(OpCodes.Ldstr, value.ToString());
			}
			else if (value is Int32)
			{
                OpCode code;

                if( !LdcOpCodesDictionary.Instance.TryGetValue( value.GetType(), out code ) )
                    code = new OpCode();

				gen.Emit(code, (int) value);
			}
			else if (value is bool)
			{
                OpCode code;

                if( !LdcOpCodesDictionary.Instance.TryGetValue( value.GetType(), out code ) )
                    code = new OpCode();

                gen.Emit(code, Convert.ToInt32(value));
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static void EmitLoadOpCodeForDefaultValueOfType(ILGenerator gen, Type type)
		{
			if (type.IsPrimitive)
			{
				gen.Emit(LdcOpCodesDictionary.Instance[type], 0);
			}
			else
			{
				gen.Emit(OpCodes.Ldnull);
			}
		}

		public static void EmitLoadIndirectOpCodeForType(ILGenerator gen, Type type)
		{
			if (type.IsEnum)
			{
				EmitLoadIndirectOpCodeForType(gen, GetUnderlyingTypeOfEnum(type));
				return;
			}

			if (type.IsByRef)
			{
				throw new NotSupportedException("Cannot load ByRef values");
			}
			else if (type.IsPrimitive)
			{
				OpCode opCode;

                if( !LdindOpCodesDictionary.Instance.TryGetValue( type, out opCode ) )
                    opCode = LdindOpCodesDictionary.EmptyOpCode;


				if (ReferenceEquals(opCode, LdindOpCodesDictionary.EmptyOpCode))
				{
					throw new ArgumentException("Type " + type + " could not be converted to a OpCode");
				}

				gen.Emit(opCode);
			}
			else if (type.IsValueType)
			{
				gen.Emit(OpCodes.Ldobj, type);
			}
			else
			{
				gen.Emit(OpCodes.Ldind_Ref);
			}
		}

		public static void EmitStoreIndirectOpCodeForType(ILGenerator gen, Type type)
		{
			if (type.IsEnum)
			{
				EmitStoreIndirectOpCodeForType(gen, GetUnderlyingTypeOfEnum(type));
				return;
			}

			if (type.IsByRef)
			{
				throw new NotSupportedException("Cannot store ByRef values");
			}
			else if (type.IsPrimitive)
			{
				OpCode opCode;

                if( !StindOpCodesDictionary.Instance.TryGetValue( type, out opCode ) )
                    opCode = StindOpCodesDictionary.EmptyOpCode;

				if (ReferenceEquals(opCode, StindOpCodesDictionary.EmptyOpCode))
				{
					throw new ArgumentException("Type " + type + " could not be converted to a OpCode");
				}

				gen.Emit(opCode);
			}
			else if (type.IsValueType)
			{
				gen.Emit(OpCodes.Stobj, type);
			}
			else
			{
				gen.Emit(OpCodes.Stind_Ref);
			}
		}

		private static Type GetUnderlyingTypeOfEnum(Type enumType)
		{
			Enum baseType = (Enum) Activator.CreateInstance(enumType);
			TypeCode code = baseType.GetTypeCode();

			switch(code)
			{
				case TypeCode.SByte:
					return typeof(SByte);
				case TypeCode.Byte:
					return typeof(Byte);
				case TypeCode.Int16:
					return typeof(Int16);
				case TypeCode.Int32:
					return typeof(Int32);
				case TypeCode.Int64:
					return typeof(Int64);
				case TypeCode.UInt16: 
					return typeof(UInt16);
				case TypeCode.UInt32:
					return typeof(UInt32);
				case TypeCode.UInt64:
					return typeof(UInt64); 
				default:
					throw new NotSupportedException();
			}
		}
	}
}