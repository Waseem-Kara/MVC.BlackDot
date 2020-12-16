using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace InvestigationApp.Tests.Controllers
{
    public class ControllerTestData
    {
        public class NullObject : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
