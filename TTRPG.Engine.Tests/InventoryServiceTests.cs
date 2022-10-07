using NUnit.Framework;
using System;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(InventoryService))]
	public class InventoryServiceTests
	{
		IInventoryService Service;

		[SetUp]
		public void SetupTest()
		{
			Service = new InventoryService();
		}

		[Test]
		public void Equip_ItemIsInBag_Equipped()
		{
			var entity = new Role();
			var item = new Role("a");
			entity.Bag.Add(item);

			Service.Equip(entity, "a", "b");

			// equipped and removed from bag
			Assert.That(entity.InventoryItems, Contains.Key("b"));
			Assert.That(entity.Bag, Does.Not.Contain(item));
		}

		[Test]
		public void Equip_ItemNotInBag_Throws()
		{
			var entity = new Role();

			Assert.Throws<Exception>(() => Service.Equip(entity, "a", "b"));
		}

		[Test(Description = "If there is already an item equipped in the slot, it should be unequipped and added to the bag before equipping the new item")]
		public void Equip_SlotFilled_Swaps()
		{
			var entity = new Role();
			var itema = new Role("a");
			var itemb = new Role("b");
			entity.InventoryItems["slot1"] = itema;
			entity.Bag.Add(itemb);

			Service.Equip(entity, "b", "slot1");

			Assert.That(entity.InventoryItems["slot1"], Is.EqualTo(itemb));
			Assert.That(entity.Bag, Does.Contain(itema));
		}

		[Test]
		public void Unequip_ItemEquipped_PutInBag()
		{
			var entity = new Role();
			var item = new Role("a");
			entity.InventoryItems["b"] = item;

			Service.Unequip(entity, "b");

			// unequipped and added to bag
			Assert.That(entity.InventoryItems, Does.Not.ContainKey("b"));
			Assert.That(entity.Bag, Does.Contain(item));
		}

		[Test]
		public void Unequip_ItemNotEquipped_Throws()
		{
			var entity = new Role();

			Assert.Throws<Exception>(() => Service.Unequip(entity, "b"));
		}

		[Test]
		public void Drop_ItemInBag_Dropped()
		{
			var entity = new Role();
			var item = new Role("a");
			entity.Bag.Add(item);

			Service.DropItem(entity, "a");

			Assert.That(entity.InventoryItems, Does.Not.Contain(item));
		}

		[Test]
		public void Drop_ItemNotInBag_Throws()
		{
			var entity = new Role();

			Assert.Throws<Exception>(() => Service.DropItem(entity, "b"));
		}

		[Test]
		public void PickupItem_ItemAddedToBag()
		{
			var entity = new Role();
			var item = new Role("a");

			Service.PickupItem(entity, item);

			Assert.That(entity.Bag, Does.Contain(item));
		}
	}
}
