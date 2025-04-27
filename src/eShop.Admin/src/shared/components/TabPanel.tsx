import { Box } from "@mui/material";
import { JSX, ReactNode } from "react";

type Props = {
    children?: ReactNode,
    value: number,
    index: number,
}

export default function TabPanel({ children, value, index, ...other }: Props): JSX.Element {
  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`product-tabpanel-${index}`}
      aria-labelledby={`product-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}