using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Assets.Scripts.HexCore.Tests
{
    public class HexCore_Tests
    {
        [UnityTest]
        public IEnumerator TestTrueAssertion()
        {
            yield return null;

            Assert.IsTrue(true);
        }
    }
}