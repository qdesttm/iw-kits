import { Flex } from 'antd';
import { RocketOutlined } from '@ant-design/icons';
import { colors } from '@/shared/config/theme';

interface LogoProps {
  collapsed: boolean;
}

export const Logo = ({ collapsed }: LogoProps) => {  return (
    <Flex
      align="center"
      justify="center"
      gap={8}
      style={{ height: 32, margin: 16 }}
    >
      <RocketOutlined style={{ fontSize: 24, color: colors.primary }} />

      {!collapsed && (
        <span style={{ fontWeight: 'bold', fontSize: 16, color: colors.sidebarText }}>
          Wellness Kits
        </span>
      )}
    </Flex>
  );
};
