﻿using NJasmineTests.Core;
using NUnit.Framework;

namespace NJasmineTests.PassingFixtures
{
    [Explicit, TestFixture]
    public class some_Nunit_fixture_a
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            imports_NUnit_fixture.Trace("FixtureSetup " + TraceableNJasmineFixture.GetTypeShortName(this.GetType()));
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            imports_NUnit_fixture.Trace("FixtureTearDown " + TraceableNJasmineFixture.GetTypeShortName(this.GetType()));
        }

        [SetUp]
        public void SetUp()
        {
            imports_NUnit_fixture.Trace("SetUp " + TraceableNJasmineFixture.GetTypeShortName(this.GetType()));
        }

        [TearDown]
        public void TearDown()
        {
            imports_NUnit_fixture.Trace("TearDown " + TraceableNJasmineFixture.GetTypeShortName(this.GetType()));
        }
    }

    [Explicit, TestFixture]
    public class some_Nunit_fixture_b : some_Nunit_fixture_a
    {
    }

    [Explicit, TestFixture]
    public class some_Nunit_fixture_c : some_Nunit_fixture_a
    {
    }

    [RunExternal(true, ExpectedTraceSequence = @"
test started, before include of a
FixtureSetup some_Nunit_fixture_a
SetUp some_Nunit_fixture_a
after include of a
first describe, before include of b
FixtureSetup some_Nunit_fixture_b
SetUp some_Nunit_fixture_b
after include of b
first test
TearDown some_Nunit_fixture_b
TearDown some_Nunit_fixture_a
test started, before include of a
SetUp some_Nunit_fixture_a
after include of a
first describe, before include of b
SetUp some_Nunit_fixture_b
after include of b
before include of c
FixtureSetup some_Nunit_fixture_c
SetUp some_Nunit_fixture_c
after include of c
second test test
TearDown some_Nunit_fixture_c
TearDown some_Nunit_fixture_b
TearDown some_Nunit_fixture_a
FixtureTearDown some_Nunit_fixture_c
FixtureTearDown some_Nunit_fixture_b
FixtureTearDown some_Nunit_fixture_a
")]
    public class imports_NUnit_fixture : TraceableNJasmineFixture
    {
        public override void Specify()
        {
            beforeAll(ResetTracing);

            Trace("test started, before include of a");
            var firstA = importNUnit<some_Nunit_fixture_a>();
            Trace("after include of a");

            arrange(() => Assert.That(firstA is some_Nunit_fixture_a));

            describe("a block", () =>
            {
                Trace("first describe, before include of b");
                var firstB = importNUnit<some_Nunit_fixture_b>();
                Trace("after include of b");

                arrange(() => Assert.That(firstB is some_Nunit_fixture_b));

                it("check fixtures", () =>
                {
                    Trace("first test");
                });

                describe("a sub block", () =>
                {
                    Trace("before include of c");
                    var secondA = importNUnit<some_Nunit_fixture_c>();
                    Trace("after include of c");

                    arrange(() => Assert.That(secondA is some_Nunit_fixture_a));
                    arrange(() => Assert.That(secondA, Is.Not.SameAs(firstA)));

                    it("check fixtures again", () =>
                    {
                        Trace("second test test");
                    });
                });
            });
        }
    }

}
