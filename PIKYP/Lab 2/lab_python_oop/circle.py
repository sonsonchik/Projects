import math
from .figure import Figure
from .color import Color

class Circle(Figure):
    """Круг."""
    FIGURE_NAME = "Круг"

    def __init__(self, radius, color):
        self.radius = radius
        self.color = Color(color)

    def area(self):
        return math.pi * self.radius ** 2

    def get_name(self):
        return self.FIGURE_NAME

    def __repr__(self):
        return "{}: радиус={}, цвет={}, площадь={:.2f}".format(
            self.get_name(), self.radius,
            self.color.color, self.area()
        )