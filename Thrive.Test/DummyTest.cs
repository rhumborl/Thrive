/*
 * >>> Investigation results
 *
 * 1. Unit tests must be stored separate from the main project (probably a good idea anyway).
 *    If not, tests are not discovered by the console runner.
 *    I was running it in Visual Studio 2019 - Test Explorer discovered them but Run All Tests came up with 0 tests found.
 * 2. Tests can reference code in the main Thrive assembly and run successfully for non-Godot code.
 *    This should mean the majority of the core engine can be made testable.
 * 3. If any Godot code is executed during a test it will fail, as it is run outside the Godot environment.
 *    This produces a similar result to the OP in https://github.com/godotengine/godot-proposals/issues/432
 *
 * Best suggestion I have currently is to make UI components as much a facade as possible,
 *   i.e. abstract Godot classes such as InputEvent to our own versions and pass them to core classes instead.
 */

namespace Thrive.Test
{
    using System.Collections.Generic;
    using Godot;
    using NUnit.Framework;

    [TestFixture]
    public class DummyTest
    {
        /// <summary>
        /// Checks basic NUnit functionality.
        /// </summary>
        [Test]
        public void SimpleTest()
        {
            Assert.AreEqual(1, 1);
        }

        /// <summary>
        /// Tests referencing Thrive but not using Godot.
        /// </summary>
        [Test]
        public void Thrive_Non_Godot()
        {
            var values = new Dictionary<string, float>()
            {
                { "a", 1.0f },
                { "b", 2.0f },
                { "c", 3.0f },
            };

            var result = global::DictionaryUtils.SumValues(values);

            Assert.AreEqual(6.0f, result);
        }

        /// <summary>
        /// Tests using something that references Godot.
        /// https://github.com/godotengine/godot-proposals/issues/432 suggests this will kaboom - and it does.
        /// </summary>
        [Test]
        public void Thrive_With_Godot()
        {
            /*
             * Message:
             * System.TypeInitializationException : The type initializer for 'Godot.InputEventKey' threw an exception.
             *     ----> System.Security.SecurityException : ECall methods must be packaged into a system module.
             * Stack Trace:
             *   InputEventKey.ctor()
             */
            var dummyGodotEvent = new InputEventKey()
            {
                Pressed = true,
            };

            var trigger = new global::InputTrigger("some_action");
            var result = trigger.CheckInput(dummyGodotEvent);

            Assert.AreEqual(true, result);
        }
    }
}
