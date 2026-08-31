using Microsoft.AspNetCore.Mvc;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The controller Plan 03's story 3 posts to — one action whose argument has a validator, and one whose
/// argument has none.
/// </summary>
/// <remarks>
/// Marked <c>[ApiController]</c> because that is how the story is written, which also puts MVC's own
/// <c>ModelStateInvalidFilter</c> in front of PineGuard's: an argument that fails model binding is answered
/// by MVC before validation is ever asked, and an argument that binds is answered by PineGuard. Binding
/// sources are stated rather than inferred so the routes read the same as the requests that reach them.
/// </remarks>
/// <seealso cref="MustValidationActionFilter"/>
[ApiController]
public sealed class SampleMvcController : ControllerBase
{
    /// <summary>
    /// Accepts an order whose body has a registered validator.
    /// </summary>
    /// <param name="order">The bound request body.</param>
    [HttpPost("/mvc/orders")]
    public IActionResult PostOrder([FromBody] CreateOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return Ok(order.Email);
    }

    /// <summary>
    /// Accepts a customer, whose type has no validator anywhere — the proof that an action binding only
    /// unvalidated types runs untouched.
    /// </summary>
    /// <param name="customer">The bound request body.</param>
    [HttpPost("/mvc/customers")]
    public IActionResult PostCustomer([FromBody] Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        return Ok(customer.Name);
    }
}
