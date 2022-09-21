using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadyPixel.Input
{
	public static class InputManager
	{
		private static InputActions _inputActions;

		public static InputActions InputActions
		{
			get
			{
				if (_inputActions == null)
				{
					_inputActions = new InputActions();
					_inputActions.Enable();
				}

				return _inputActions;
			}
		}
	}
}