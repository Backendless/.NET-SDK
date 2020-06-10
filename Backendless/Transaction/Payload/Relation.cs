using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Payload
{
  public class Relation : Selector
  {
    public Relation() : base()
    {
    }

    public Relation( String conditional, Object unconditional, Object parentObject, String relationColumn,
                     List<String> objectIds, String relationTableName, bool columnUnique ) : base( conditional, unconditional )
    {
      ParentObject = parentObject;
      RelationColumn = relationColumn;
      ObjectIds = objectIds;
      RelationTableName = relationTableName;
      ColumnUnique = columnUnique;
    }

    [SetClientClassMemberName("parentObject")]
    public Object ParentObject { get; set; }

    [SetClientClassMemberName("relationColumn")]
    public String RelationColumn{ get; set; }

    [SetClientClassMemberName("objectIds")]
    public List<String> ObjectIds { get; set; }

    [SetClientClassMemberName("relationTableName")]
    public String RelationTableName { get; set; }

    [SetClientClassMemberName("columnUnique")]
    public bool ColumnUnique { get; set; }

    public override string ToString()
    {
      return "Relation{" +
            "parentObject=" + ParentObject + '\'' +
            ", relationColumn='" + RelationColumn + '\'' +
            ", conditional='" + Conditional + '\'' +
            ", unconditional=" + Unconditional +
            '}';
    }

  }
}
