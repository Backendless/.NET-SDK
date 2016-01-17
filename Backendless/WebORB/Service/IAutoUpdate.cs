using System;

namespace Weborb.Service
{
	public interface IAutoUpdate
	{
		IUpdateHandler GetUpdateHandler();
	}
}
