import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Dashboard from '../pages/dashboard/DashboardPage';
import ProtectedRoute from './ProtectedRoute';
import { AppLayout } from '../components/layout/AppLayout';
import IngredientsPage from '../pages/ingredient/IngredientsPage';
import RecipesPage from '../pages/recipe/RecipesPage';
import IngredientDetailsPage from '../pages/ingredient/IngredientDetailsPage';
import IngredientCreatePage from '../pages/ingredient/IngredientCreatePage';
import RecipeCreatePage from '../pages/recipe/RecipeCreatePage';
import RecipeDetailsPage from '../pages/recipe/RecipeDetailsPage';
import RestaurantsPage from '../pages/restaurant/RestaurantsPage';
import RestaurantCreatePage from '../pages/restaurant/RestaurantCreatePage';
import RestaurantDetailsPage from '../pages/restaurant/RestaurantDetailsPage';

const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
   
        <Route
            path="/"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <Dashboard />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/ingredientes/novo"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <IngredientCreatePage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/ingredientes/:id"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <IngredientDetailsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/ingredientes"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <IngredientsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/receitas"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <RecipesPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/receitas/:id"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <RecipeDetailsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/receitas/nova"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <RecipeCreatePage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/restaurantes"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <RestaurantsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/restaurantes/novo"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <RestaurantCreatePage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/restaurantes/:id"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <RestaurantDetailsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;
