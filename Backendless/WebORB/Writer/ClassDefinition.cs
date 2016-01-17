using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Weborb.Writer
{
    public class ClassDefinition
    {
        //private List<MemberInfo> members;
        //private List<ITypeWriter> writers;
        private Dictionary<string, MemberInfo> nameToMember;
        private string className;

        public ClassDefinition()
        {
            //members = new List<MemberInfo>();
            //writers = new List<ITypeWriter>();
            nameToMember = new Dictionary<string, MemberInfo>();
        }

        public Dictionary<string, MemberInfo> Members
        {
            get
            {
                return nameToMember;
            }
        }

        /*
        public MemberInfo[] Members
        {
            get
            {
                return members.ToArray();
            }
        }

        public ITypeWriter[] Writers
        {
            get
            {
                return writers.ToArray();
            }
        }
        */

        public void AddMemberInfo( string memberName, MemberInfo memberInfo )
        {
            //members.Add( memberInfo );
            //writers.Add( writer );
            nameToMember.Add(memberName, memberInfo);
        }

        public bool ContainsMember( string name )
        {
            return nameToMember.ContainsKey(name);

            /*foreach( MemberInfo member in members )
                if( member.Name.Equals( name ) )
                    return true;

            return false;*/
        }

        public string ClassName
        {
            get
            {
                return className;
            }

            set
            {
                this.className = value;
            }
        }
    }
}
