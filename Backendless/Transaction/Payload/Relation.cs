using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction.Payload
{
  public class Relation : Selector
  {
    private Object parentObject;
    private String relationColumn;

    //helpers
    [JsonIgnore]
    private List<String> objectIds;
    [JsonIgnore]
    private String relationTableName;
    [JsonIgnore]
    private bool columnUnique;

    public Relation() : base()
    {
    }

    public Relation( String conditional, Object unconditional, Object parentObject, String relationColumn,
                     List<String> objectIds, String relationTableName, bool columnUnique ) : base( conditional, unconditional )
    {
      this.parentObject = parentObject;
      this.relationColumn = relationColumn;
      this.objectIds = objectIds;
      this.relationTableName = relationTableName;
      this.columnUnique = columnUnique;
    }

    public Object ParentObject
    {
      get => parentObject;
      set => parentObject = value;
    }

    public String RelationColumn
    {
      get => relationColumn;
      set => relationColumn = value;
    }

    public List<String> ObjectIds
    {
      get => objectIds;
      set => objectIds = value;
    }

    public String RelationTableName
    {
      get => relationTableName;
      set => relationTableName = value;
    }

    public bool ColumnUnique
    {
      get => columnUnique;
      set => columnUnique = value;
    }

    public override string ToString()
    {
      return "Relation{" +
            "parentObject=" + parentObject + '\'' +
            ", relationColumn='" + relationColumn + '\'' +
            ", conditional='" + Conditional + '\'' +
            ", unconditional=" + Unconditional +
            '}';
    }

  }
}
