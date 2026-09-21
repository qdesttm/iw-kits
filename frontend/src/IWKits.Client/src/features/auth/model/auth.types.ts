export interface AuthUser {
  id: string;
  username: string;
  role: string;
}

export interface AuthResponse {
  access_token: string | null;
  refresh_token: string | null;
  user: AuthUser | null;
  error_message: string | null;
}

export interface RefreshResponse {
  access_token: string | null;
  refresh_token: string | null;
  error_message: string | null;
}

export interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  loading: boolean;
  login: (username: string, password: string) => Promise<string | null>;
  logout: () => void;
}
