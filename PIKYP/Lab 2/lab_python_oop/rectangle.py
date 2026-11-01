from .figure import Figure
from .color import Color

class Rectangle(Figure):
    """Прямоугольник."""
    FIGURE_NAME = "Прямоугольник"

    def __init__(self, width, height, color):
        self.width = width
        self.height = height
        self.color = Color(color)

    def area(self):
        return self.width * self.height

    def get_name(self):
        return self.FIGURE_NAME

    def __repr__(self):
        return "{}: ширина={}, высота={}, цвет={}, площадь={:.2f}".format(
            self.get_name(), self.width, self.height,
            self.color.color, self.area()
        )