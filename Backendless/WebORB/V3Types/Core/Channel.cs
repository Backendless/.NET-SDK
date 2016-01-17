using System;
using System.Collections;

namespace Weborb.V3Types.Core
  {
  public class Channel 
  {
  	private String id;
  	private String endpointUri;
  	private String endpointClass = "";
  	private String channelClass = "";
  	private Hashtable properties = new Hashtable();
  	
  	public Channel( String id, String endpointUri )
  	{
  		this.id = id;
  		this.endpointUri = endpointUri;
  	}
  	
  	public String getId()
  	{
  		return id;		
  	}
  	
  	public void setId( String id )
  	{
  		this.id = id;		
  	}
  	
  	public String getEndpointClass()
  	{
  		return endpointClass;		
  	}
  	
  	public void setEndpointClass( String endpointClass )
  	{
  		this.endpointClass = endpointClass;		
  	}
  	
  	public String getEndpointUri()
  	{
  		return endpointUri;		
  	}
  	
  	public void setEndpointUri( String endpointUri )
  	{
  		this.endpointUri = endpointUri;		
  	}
  	
  	public String getChannelClass()
  	{
  		return channelClass;		
  	}
  	
  	public void setChannelClass( String channelClass )
  	{
  		this.channelClass = channelClass;		
  	}
  	
  	public Hashtable getProperties()
  	{
  		return properties;		
  	}
  	
  	public void setProperties( Hashtable properties )
  	{
  		this.properties = properties;		
  	}
  }
}