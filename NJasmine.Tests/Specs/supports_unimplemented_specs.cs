﻿using System;
using NJasmineTests.Core;
using NJasmineTests.Export;

namespace NJasmineTests.Specs
{
    public class supports_unimplemented_specs : GivenWhenThenFixtureTracingToConsole, INJasmineInternalRequirement
    {
        public override void Specify()
        {
            beforeAll(ResetTracing);

            it("an unimplemented test() block");

            describe("nested too of course", delegate
            {
                it("an unimplemented test() block");
            });
        }

        public void Verify(FixtureResult fixtureResult)
        {
            fixtureResult.hasTest("NJasmineTests.Specs.supports_unimplemented_specs, an unimplemented test() block").thatIsNotRunnable();

            fixtureResult.hasTest("NJasmineTests.Specs.supports_unimplemented_specs, nested too of course, an unimplemented test() block").thatIsNotRunnable();
        }
    }
}
