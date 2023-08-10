using System;
using UnityEngine;

namespace UI.Base.Widgets
{
    public abstract class LayoutData<T> : IEquatable<T> where T : LayoutData<T>
    {
        public string Title { get; }
        public Sprite Sprite { get; }
        public bool IsSelected { get; set; }

        protected LayoutData(string title, Sprite sprite = null, bool isSelected = false)
        {
            Title = title;
            Sprite = sprite;
            IsSelected = isSelected;
        }
        
        protected LayoutData(Sprite sprite, bool isSelected = false)
        {
            Sprite = sprite;
            IsSelected = isSelected;
        }
        
        protected virtual bool IsEqualTo(T other) =>
            Equals(Title, other.Title) 
            && Equals(Sprite, other.Sprite) 
            && IsSelected == other.IsSelected;

        protected virtual int HashCode => System.HashCode.Combine(
            Title,
            Sprite, 
            IsSelected);
        
        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsEqualTo(other);
        }

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != GetType()) return false;
            return Equals((T)obj);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}