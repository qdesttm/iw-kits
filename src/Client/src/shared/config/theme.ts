export const colors = {
  primary: '#1677ff',
  pageBackground: '#f5f5f5',
  textMuted: '#666666',
  sidebarText: '#ffffff',
  sidebarTextMuted: '#ffffffa6',
  authGradientFrom: '#667eea',
  authGradientTo: '#764ba2',
} as const;

export const radius = {
  card: 8,
  panel: 12,
} as const;

export const authGradient = `linear-gradient(135deg, ${colors.authGradientFrom} 0%, ${colors.authGradientTo} 100%)`;
