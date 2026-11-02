import unittest
import math
from quadratic import solve_square, get_roots


class TestSolveSquare(unittest.TestCase):
    """TDD-тесты для квадратного уравнения."""

    def test_two_roots(self):
        """D > 0: два различных корня."""
        roots = solve_square(1, -5, 4)
        self.assertEqual(len(roots), 2)
        self.assertIn(1.0, [round(r, 6) for r in roots])
        self.assertIn(4.0, [round(r, 6) for r in roots])

    def test_one_root(self):
        """D == 0: один корень."""
        roots = solve_square(1, -2, 1)
        self.assertEqual(len(roots), 1)
        self.assertAlmostEqual(roots[0], 1.0)

    def test_no_roots(self):
        """D < 0: нет действительных корней."""
        roots = solve_square(1, 1, 1)
        self.assertEqual(roots, [])

    def test_a_is_zero_raises(self):
        """A == 0: должно возбуждаться исключение."""
        with self.assertRaises(ValueError):
            solve_square(0, 1, 1)


class TestGetRoots(unittest.TestCase):
    """TDD-тесты для биквадратного уравнения."""

    def test_four_roots(self):
        """x^4 - 5x^2 + 4 = 0 -> четыре корня: ±1, ±2."""
        roots = get_roots(1, -5, 4)
        self.assertEqual(len(roots), 4)
        rounded = sorted(round(r, 6) for r in roots)
        self.assertEqual(rounded, [-2.0, -1.0, 1.0, 2.0])

    def test_two_roots(self):
        """x^4 - 4 = 0 -> два корня: ±sqrt(2)."""
        roots = get_roots(1, 0, -4)
        self.assertEqual(len(roots), 2)
        expected = round(math.sqrt(2), 6)
        for r in roots:
            self.assertAlmostEqual(abs(r), expected, places=6)

    def test_one_root(self):
        """x^4 = 0 -> один корень 0."""
        roots = get_roots(1, 0, 0)
        self.assertEqual(roots, [0.0])

    def test_no_roots(self):
        """x^4 + x^2 + 1 = 0 -> нет действительных корней."""
        roots = get_roots(1, 1, 1)
        self.assertEqual(roots, [])

    def test_a_is_zero_raises(self):
        """A == 0: должно возбуждаться исключение."""
        with self.assertRaises(ValueError):
            get_roots(0, -5, 7)


if __name__ == '__main__':
    unittest.main()