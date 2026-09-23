import type { ThemeConfig } from 'antd';
import { colors, radius } from '@/shared/config/theme';

export const theme: ThemeConfig = {
  token: {
    colorPrimary: colors.primary,
    borderRadius: radius.card,
  },
};
