using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class EmailEnvelope
  {
    [SetClientClassMemberName("cc")]
    public List<string> Cc { get; set; }
    
    [SetClientClassMemberName("bcc")]
    public List<string> Bcc { get; set; }
    
    [SetClientClassMemberName("to")]
    public List<string> To { get; set; }
    
    [SetClientClassMemberName("query")]
    public string RecipientsQuery { get; set; }

    [SetClientClassMemberName("uniqueEmails")]
    public Boolean UniqueEmails { get; set; } = true;

    public EmailEnvelope AddTo( IEnumerable<string> toAddresses )
    {
      if( To == null )
        To = new List<string>();
      
      To.AddRange( toAddresses );
      return this;
    }

    public EmailEnvelope SetTo( List<string> toAddresses )
    {
      To = toAddresses;
      return this;
    }
    
    public EmailEnvelope AddCc( IEnumerable<string> ccAddresses )
    {
      if( Cc == null )
        Cc = new List<string>();

      Cc.AddRange( ccAddresses );
      return this;
    }

    public EmailEnvelope SetCc( List<string> ccAddresses )
    {
      Cc = ccAddresses;
      return this;
    }


    public EmailEnvelope AddBcc( IEnumerable<string> bccAddresses )
    {
      if( Bcc == null )
        Bcc = new List<string>();

      Bcc.AddRange( bccAddresses );
      return this;
    }

    public EmailEnvelope SetBcc( List<string> bccAddresses )
    {
      Bcc = bccAddresses;
      return this;
    }
  }
}