using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  abstract class Geometry<T>
  {
    abstract public String GetGeoJSONType();

    abstract public String GetWKTType();

    abstract internal String JSONCoordinatePairs();

    abstract internal String WKTCoordinatePairs();

    public String AsGeoJSON()
    {
      return "{\"type\":\"" + this.GetGeoJSONType() + "\",\"coordinates:\":" + this.JSONCoordinatePairs() + "}";
    }

    public String AsWKT()
    {
      return GetWKTType() + $"({this.WKTCoordinatePairs()})";
    }

    public override string ToString()
    {
      return $"'{AsWKT()}'";
    }
  }
}
