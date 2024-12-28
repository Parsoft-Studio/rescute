using System;

namespace rescute.Domain.Exceptions;

public class MobilePhoneExpectedException() : Exception("Only mobile phone numbers are acceptable.");