﻿namespace KoiAuction.API.Controllers.ResponseTypes
{
    /// <summary>
    /// Implicit wrapping of types that serialize to non-complex values.
    /// </summary>
    /// <typeparam name="T">Types such as string, Guid, int, long, etc.</typeparam>
    public class JsonResponse<T>
    {
        public JsonResponse(string message, T value)
        {
            Message = message;
            Value = value;
        }
        public string Message { get; set; }
        public T Value { get; set; }
    }
}
