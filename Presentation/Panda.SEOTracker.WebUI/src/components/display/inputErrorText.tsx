import { Typography, theme } from 'antd';
import React from 'react';
import { ValidationError } from '../appHttpClient';

const { Text } = Typography;
const { useToken } = theme

interface IProps {
    propertyNames: string[]
    validationErrors: ValidationError[]
}
const ErrorText: React.FC<IProps> = (props) => {
    const { token } = useToken()
    return (<>
        {props?.validationErrors?.filter(x => props.propertyNames?.includes(x.propertyName)).map(
            (x, index) => <Text key={index} style={{ color: token.colorErrorText }}>{x.message} </Text>)}
    </>);
};

export default ErrorText;