﻿using System;
using System.Collections.Generic;

namespace DeliverySimulator.Kitchen.Models
{
    /// <summary>
    /// <see cref="ShelfOrder"/> container based on <see cref="LinkedList{ShelfOrder}"/> 
    /// </summary>
    public class KitchenShelf
    {
        private object _syncLock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp">Shelf identifier - temperature</param>
        /// <param name="maxCapacity">Max capacity of the inner container</param>
        /// <param name="decayModifier">Decay modifier for orders. Affects order deterrioration rate.</param>
        public KitchenShelf(string temp, int maxCapacity, int decayModifier)
        {
            Temp = temp;
            MaxCapacity = maxCapacity;
            DecayModifier = decayModifier;
            Orders = new List<ShelfOrder>();
        }

        /// <summary>
        /// Temperature of the inner orders. Shelf identifier.
        /// </summary>
        public string Temp { get; }

        /// <summary>
        /// Represents maximum number of elements inside.
        /// </summary>
        public int MaxCapacity { get; }

        /// <summary>
        /// Decay modifier for orders inside shelf. Affects deterrioration rate.
        /// </summary>
        public int DecayModifier { get; }

        /// <summary>
        /// Orders inside shelf.
        /// </summary>
        public List<ShelfOrder> Orders { get; }

        

        /// <summary>
        /// Adds order to shelf. Element can not be added if shelf reached its MaxCapacity
        /// </summary>
        /// <param name="order">Order to be added</param>
        /// <returns>true, if order was added, otherwise - false</returns>
        public bool Add(ShelfOrder order)
        {
            lock (_syncLock)
            {
                if (Orders.Count >= MaxCapacity)
                {
                    return false;
                }

                Orders.Add(order);

                return true;
            }
        }

        /// <summary>
        /// Removes random element from shelf
        /// </summary>
        /// <returns>Shelf order that was removed</returns>
        public ShelfOrder RemoveRandom()
        {
            lock (_syncLock)
            {
                var rand = new Random();
                var elementIndexToRemove = rand.Next(0, Orders.Count - 1);
                var order = Orders[elementIndexToRemove];

                Orders.Remove(order);

                return order;
            }
        }

        /// <summary>
        /// Removes <see cref="ShelfOrder"/> from the shelf
        /// </summary>
        /// <param name="order">Order to be removed</param>
        /// <returns>true, if order was removed, otherwise - false</returns>
        public bool Remove(ShelfOrder order)
        {
            lock (_syncLock)
            {
                return Orders.Remove(order);
            }
        }
    }
}
