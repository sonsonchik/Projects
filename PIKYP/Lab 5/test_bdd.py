import pytest
from pytest_bdd import scenarios, given, when, then
from quadratic import get_roots

scenarios('features/quadratic.feature')

@pytest.fixture
def context():
    return {}

@given('коэффициенты A=1 B=-5 C=4')
def step_impl_1(context):
    context['a'], context['b'], context['c'] = 1.0, -5.0, 4.0

@given('коэффициенты A=1 B=0 C=-4')
def step_impl_2(context):
    context['a'], context['b'], context['c'] = 1.0, 0.0, -4.0

@given('коэффициенты A=1 B=1 C=1')
def step_impl_3(context):
    context['a'], context['b'], context['c'] = 1.0, 1.0, 1.0

@given('коэффициенты A=0 B=1 C=1')
def step_impl_4(context):
    context['a'], context['b'], context['c'] = 0.0, 1.0, 1.0

@when('я вычисляю корни')
def step_calculate(context):
    try:
        context['roots'] = get_roots(context['a'], context['b'], context['c'])
        context['error'] = None
    except ValueError as e:
        context['roots'] = None
        context['error'] = e

@then('должно быть получено 4 корня')
def step_check_4(context):
    assert len(context['roots']) == 4

@then('должно быть получено 2 корня')
def step_check_2(context):
    assert len(context['roots']) == 2

@then('должно быть получено 0 корней')
def step_check_0(context):
    assert len(context['roots']) == 0

@then('корни должны быть -2, -1, 1, 2')
def step_check_values(context):
    expected = [-2.0, -1.0, 1.0, 2.0]
    actual = sorted(round(r, 6) for r in context['roots'])
    assert actual == expected

@then('должно возникнуть исключение ValueError')
def step_check_error(context):
    assert context['error'] is not None
    assert isinstance(context['error'], ValueError)