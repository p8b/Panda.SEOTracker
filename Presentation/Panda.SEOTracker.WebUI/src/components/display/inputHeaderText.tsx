import { Typography } from 'antd';
import React from 'react';

const { Text } = Typography;

interface IProps {
    Title: string,
}
const ErrorText: React.FC<IProps> = (props) => {
    return (<>
        <Text>{props.Title} </Text>
    </>);
};

export default ErrorText;