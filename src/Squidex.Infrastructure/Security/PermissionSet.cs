﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Squidex.Infrastructure.Security
{
    public sealed class PermissionSet : IReadOnlyCollection<Permission>
    {
        public static readonly PermissionSet Empty = new PermissionSet(new string[0]);

        private readonly List<Permission> permissions;
        private readonly Lazy<string> display;

        public int Count
        {
            get { return permissions.Count; }
        }

        public PermissionSet(params Permission[] permissions)
            : this((IEnumerable<Permission>)permissions)
        {
        }

        public PermissionSet(params string[] permissions)
            : this(permissions?.Select(x => new Permission(x)))
        {
        }

        public PermissionSet(IEnumerable<string> permissions)
            : this(permissions?.Select(x => new Permission(x)))
        {
        }

        public PermissionSet(IEnumerable<Permission> permissions)
        {
            Guard.NotNull(permissions, nameof(permissions));

            this.permissions = permissions.ToList();

            display = new Lazy<string>(() => string.Join(";", this.permissions));
        }

        public bool Allows(Permission other)
        {
            if (other == null)
            {
                return false;
            }

            return permissions.Any(x => x.Allows(other));
        }

        public bool Includes(Permission other)
        {
            if (other == null)
            {
                return false;
            }

            return permissions.Any(x => x.Includes(other));
        }

        public override string ToString()
        {
            return display.Value;
        }

        public IEnumerable<string> ToIds()
        {
            return permissions.Select(x => x.Id);
        }

        public IEnumerator<Permission> GetEnumerator()
        {
            return permissions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return permissions.GetEnumerator();
        }
    }
}
