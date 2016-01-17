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

namespace Weborb.ProxyGen.Core
{
	using System;
	using System.Collections;
    using System.Collections.Generic;

	public enum LifecycleStepType
	{
		Commission,
		Decommission
	}

    public class LifecycleStepCollection : ICollection
	{
		private IList<Object> commissionSteps;
        private IList<Object> decommissionSteps;

		public LifecycleStepCollection()
		{
			commissionSteps = new List<object>();
            decommissionSteps = new List<object>();
		}

		public object[] GetCommissionSteps()
		{
			object[] steps = new object[commissionSteps.Count];
			commissionSteps.CopyTo(steps, 0);
			return steps;
		}

		public object[] GetDecommissionSteps()
		{
			object[] steps = new object[decommissionSteps.Count];
			decommissionSteps.CopyTo(steps, 0);
			return steps;
		}

		public bool HasCommissionSteps
		{
			get { return commissionSteps.Count != 0; }
		}

		public bool HasDecommissionSteps
		{
			get { return decommissionSteps.Count != 0; }
		}

		public void Add(LifecycleStepType type, object stepImplementation)
		{
			if (stepImplementation == null) throw new ArgumentNullException("stepImplementation");

			if (type == LifecycleStepType.Commission)
			{
				commissionSteps.Add(stepImplementation);
			}
			else
			{
				decommissionSteps.Add(stepImplementation);
			}
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return commissionSteps.Count + decommissionSteps.Count; }
		}

		public IEnumerator GetEnumerator()
		{
            List<Object> copy = new List<Object>( commissionSteps );
            return copy.GetEnumerator();
		}

        #region ICollection Members


        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}