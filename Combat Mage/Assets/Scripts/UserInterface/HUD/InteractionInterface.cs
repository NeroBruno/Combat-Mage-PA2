using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionInterface : UserInterfaceBehaviour
{
    [SerializeField]
    private Animator _MainAnimator = null;

    [SerializeField]
    private Text _MainText = null;

    private RaycastData _RaycastData;

    private ItemPickup _ItemPickup;

    public override void OnPostAttachment()
    {
        Player.RaycastData.AddChangeListener(OnPlayerRaycastChange);

    }

    private void OnPlayerRaycastChange(RaycastData raycastData)
    {
        bool show = raycastData != null && raycastData.IsInteractive;
        _RaycastData = raycastData;

        if (show)
        {
            _MainAnimator.SetBool("Show", true);

			//if (_WeaponSwapping.SwapIcon != null)
			//{
			//	bool enableSwapUI = false;
			//	_MainText.text = _RaycastData.InteractiveObject.InteractionText;

			//	// Current item
			//	SaveableItem currentItem = Player.EquippedItem.Get();

			//	if (currentItem != null && Player.ItemIsSwappable.Try(currentItem))
			//	{
			//		// Detected item
			//		ItemPickup pickup = _RaycastData.InteractiveObject as ItemPickup;

			//		//Make sure the item is swappable and check if the inventory is full
			//		enableSwapUI = pickup != null && Player.ItemIsSwappable.Try(pickup.ItemInstance) &&
			//					   Player.Inventory.GetContainerWithFlags(pickup.TargetContainers).ContainerIsFull();

			//		if (enableSwapUI)
			//		{
			//			_ItemPickup = pickup;

			//			_ItemPickup.NeedToSwap = true;

			//			_WeaponSwapping.EquippedItemImg.sprite = Player.EquippedItem.Val.Data.Icon;
			//			_WeaponSwapping.GroundItemImg.sprite = _ItemPickup.ItemInstance.Data.Icon;
			//		}
			//	}

			//	EnableSwapUI(enableSwapUI);
			//}
		}
        else
        {
			_MainAnimator.SetBool("Show", false);
			//_SwapAnimator.SetBool("Show", false);

			if (_ItemPickup != null)
            {
				//_ItemPickup.NeedToSwap = false;
				_ItemPickup = null;
            }
        }
    }

	//private void ChangeVisibilityOfSwapIcon(SaveableItem item)
	//{
	//	bool enable = (item != null);

	//	if (enable)
	//		//_WeaponSwapping.EquippedItemImg.sprite = item.Data.Icon;

	//	if (Player.RaycastData != null)
	//		EnableSwapUI(enable);
	//}

    private void Update()
    {
		//if (_ItemPickup != null)
		//	UpdateSwapFill(_ItemPickup.InteractionProgress);
    }

	private void EnableSwapUI(bool enable)
	{
		if (Player.RaycastData.Val != null)
		{
			if (Player.RaycastData.Val.IsInteractive)
			{
				_MainAnimator.SetBool("Show", !enable);
				//_SwapAnimator.SetBool("Show", enable);
			}
		}
		else
		{
			_MainAnimator.SetBool("Show", false);
			//_SwapAnimator.SetBool("Show", false);
		}

		//if (_ItemPickup != null && enable == false)
		//	_ItemPickup.NeedToSwap = false;
	}

	private void UpdateSwapFill(float amount)
	{
		//_WeaponSwapping.SwapIcon.fillAmount = amount * (1 / _ItemPickup.SwapTime);
	}

	[Serializable]
	private struct WeaponSwappingModule
	{
		public Image SwapIcon;

		public Image EquippedItemImg;

		public Image GroundItemImg;
	}
}
