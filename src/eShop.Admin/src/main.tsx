import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { Provider } from 'react-redux'
import { RouterProvider } from 'react-router-dom'
import { AuthProvider } from 'react-oidc-context'
import { router } from './app/routing/Routes'
import { store } from './app/store/store'
import { oidcConfig } from './shared/config/oidcConfig'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <AuthProvider {...oidcConfig}>
        <RouterProvider router={router} />
      </AuthProvider>
    </Provider>
  </StrictMode>,
)