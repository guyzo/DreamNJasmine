﻿using System;
using System.Linq;
using NJasmineTests.Core;
using NUnit.Framework;

namespace NJasmineTests.Specs
{
    [Explicit, RunExternal(false, ExpectedTraceSequence = @"
outer it 1
repeated inner it 1
outer it 2
repeated inner it 2
outer it 3
repeated inner it 3
", ExpectedStrings = new [ ]
    {
        "Test Error : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated outer test",
        "Test Error : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated outer test`2",
        "Test Error : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated outer test`3",
        "NotRunnable : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated unimplemented outer test",
        "NotRunnable : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated unimplemented outer test`2",
        "NotRunnable : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated unimplemented outer test`3",
        "Test Failure : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated describe, repeated inner describe",
        "Test Failure : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated describe, repeated inner describe`2",
        "Test Failure : NJasmineTests.Specs.duplicate_test_names_are_fine, repeated describe, repeated inner describe`3"
    }
)]
    public class duplicate_test_names_are_fine : GivenWhenThenFixtureTracingToConsole
    {
        public override void Specify()
        {
            beforeAll(ResetTracing);

            foreach (var i in Enumerable.Range(1, 3))
            {
                it("repeated outer test", delegate
                {
                    var value = "outer it " + i;
                    Trace(value);
                    throw new Exception(value);
                });

                it("repeated unimplemented outer test");

                describe("repeated describe", delegate
                {
                });

                describe("repeated describe", delegate
                {
                });

                describe("repeated describe", delegate
                {
                    it("repeated inner it", delegate
                    {
                        Trace("repeated inner it " + i);
                    });

                    describe("repeated inner describe", delegate
                    {
                        var numerator = 10;
                        var denominator = 0;
                        var result = numerator/denominator;
                    });
                });
            }
        }
    }
}