export interface AuthUser {
  id: string;
  username: string;
  role: string;
}

export interface TokensResponse {
  accessToken: string;
  refreshToken: string;
}

export interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  loading: boolean;
  login: (username: string, password: string) => Promise<string | null>;
  register: (username: string, password: string) => Promise<string | null>;
  logout: () => void;
}
