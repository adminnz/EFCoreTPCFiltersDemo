namespace EFCoreTPCFilters
{
    public static class TenantContext
    {
        private static readonly AsyncLocal<Guid?> CurrentValue = new();

        public static Guid TenantId => CurrentValue.Value ?? throw new InvalidOperationException("Tenant Context Not Available");
        public static Guid TenantIdOrEmpty => CurrentValue.Value ?? Guid.Empty;
        public static bool IsTenant => CurrentValue.Value.HasValue;

        public static IDisposable Scope(Guid? tenantId) => new ContextScope(tenantId);
        public static IAsyncDisposable ScopeAsync(Guid? tenantId) => new ContextScope(tenantId);

        private sealed class ContextScope : IDisposable, IAsyncDisposable
        {
            private readonly Guid? _previousValue;
            private bool _disposed;
            public ContextScope(Guid? value)
            {
                _previousValue = CurrentValue.Value;
                CurrentValue.Value = value;
            }
            ~ContextScope() => Dispose();
            public void Dispose()
            {
                if (!_disposed)
                {
                    CurrentValue.Value = _previousValue;
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
            }

            public ValueTask DisposeAsync()
            {
                if (!_disposed)
                {
                    CurrentValue.Value = _previousValue;
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
                return ValueTask.CompletedTask;
            }
        }
    }
}
