from abc import ABC, abstractmethod

class Figure(ABC):
    """Абстрактная геометрическая фигура."""

    @abstractmethod
    def area(self):
        """Вычислить площадь фигуры."""
        pass