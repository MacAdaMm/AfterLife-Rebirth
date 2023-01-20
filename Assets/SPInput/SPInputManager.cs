using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadyPixel.Input
{
	public static class SPInputManager
	{
		private static SPInputActions _inputActions;

		public static SPInputActions InputActions
		{
			get
			{
				if (_inputActions == null)
				{
					_inputActions = new SPInputActions();
					_inputActions.Enable();
				}

				return _inputActions;
			}
		}
	}
}