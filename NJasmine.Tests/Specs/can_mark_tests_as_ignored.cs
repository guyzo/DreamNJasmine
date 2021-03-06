﻿using System;
using NJasmine;
using NJasmineTests.Export;

namespace NJasmineTests.Specs
{
    public class can_mark_tests_as_ignored : GivenWhenThenFixture, INJasmineInternalRequirement
    {
        public override void Specify()
        {
            given("an outer block", delegate
            {
                when("ignore is set before all the tests", delegate
                {
                    ignoreBecause("the test requires it");

                    then("the when statement doesnt run, and the outer block is inconclusive", delegate
                    {
                    });
                });
            });

            when("ignore is set after a test", delegate
            {
                then("the earlier test runs", delegate
                {
                });

                ignoreBecause("the test requires it");

                then("the later test is ignored, and the containing when is inconclusive", delegate
                {
                });
            });
        }

        public void Verify_NJasmine_implementation(FixtureResult fixtureResult)
        {
            fixtureResult.succeeds();

            fixtureResult.hasSuite("given an outer block").thatsInconclusive().thatHasNoResults();

            fixtureResult.hasSuite("when ignore is set after a test").thatSucceeds()
                .ShouldHaveTest("NJasmineTests.Specs.can_mark_tests_as_ignored, when ignore is set after a test, then the earlier test runs")
                .thatSucceeds();
        }
    }
}
