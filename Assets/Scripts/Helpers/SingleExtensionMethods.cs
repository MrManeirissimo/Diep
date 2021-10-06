using System;

public static class SingleExtensionMethods {
    public static bool IsPositive(this Single s) {
        return s > 0;
    }

    public static bool IsNegative(this Single s) {
        return s < 0;
    }
}