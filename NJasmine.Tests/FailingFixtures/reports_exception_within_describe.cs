﻿using NJasmine;
using NUnit.Framework;

namespace NJasmineTests.FailingFixtures
{
    [Explicit, RunExternal(false, ExpectedStrings = new[] {
        "Test Failure : NJasmineTests.FailingFixtures.reports_exception_within_describe.broken describe", 
        "Attempted to divide by zero."})]
    public class reports_exception_within_describe : NJasmineFixture
    {
        public override void Tests()
        {
            it("outer test", delegate() { });

            describe("broken describe", delegate()
            {
                it("inner test", delegate()
                {
                });

                int j = 5;
                int i = 1 / (j - 5);
            });

            it("last test", delegate() { });
        }
    }
}