﻿
using System;
using System.Collections.Generic;
using NJasmine.Core.FixtureVisitor;
using NUnit.Core;

namespace NJasmine.Core
{
    public partial class NJasmineTestMethod : TestMethod, INJasmineTest, ISpecPositionVisitor
    {
        public class DescribeState : ISpecPositionVisitor
        {
            NJasmineTestMethod _subject;

            public DescribeState(NJasmineTestMethod subject)
            {
                _subject = subject;
            }

            public virtual void visitFork(SpecElement origin, string description, Action action, TestPosition position)
            {
                if (_subject._position.ToString().StartsWith(position.ToString()))
                {
                    action();
                }
            }

            public virtual void visitAfterEach(SpecElement origin, Action action, TestPosition position)
            {
                _subject._allTeardowns.Add(delegate()
                {
                    _subject.whileInState(new CleanupState(_subject, origin), action);
                });
            }

            public virtual void visitTest(SpecElement origin, string description, Action action, TestPosition position)
            {
                if (position.ToString() == _subject._position.ToString())
                {
                    _subject.whileInState(new ActState(_subject, origin), action);

                    throw new TestFinishedException();
                }
            }

            public virtual TFixture visitImportNUnit<TFixture>(TestPosition position) where TFixture : class, new()
            {
                _subject.whileInState(new CleanupState(_subject, SpecElement.importNUnit), delegate
                {
                    _subject._nUnitImports.DoSetUp(position);
                });

                _subject._allTeardowns.Add(delegate
                {
                    _subject.whileInState(new CleanupState(_subject, SpecElement.importNUnit), delegate
                    {
                        _subject._nUnitImports.DoTearDown(position);
                    });
                });

                return _subject._nUnitImports.GetInstance(position) as TFixture;
            }

            public virtual TArranged visitBeforeEach<TArranged>(SpecElement origin, string description, Func<TArranged> factory, TestPosition position)
            {
                TArranged result = default(TArranged);

                _subject.whileInState(new ArrangeState(_subject, origin), delegate
                {
                    result = factory();
                });

                if (result is IDisposable)
                {
                    _subject._allTeardowns.Add(delegate
                    {
                        _subject.whileInState(new CleanupState(_subject, origin), delegate
                        {
                            (result as IDisposable).Dispose();
                        });
                    });
                }

                return result;
            }
        }
    }
}