import React from 'react';
import MenuItem from '@material-ui/core/MenuItem';
import TextField from '@material-ui/core/TextField';
import SegmentationFormPanel from './SegmentationFormPanel/SegmentationFormPanel';

type ConfigFormProps = {
    organismName: string,
    onChange: void | null,
    config: {},
}

class ConfigForm extends React.Component<any, ConfigFormProps> {
    constructor(props: ConfigFormProps) {
        super(props);
        if (props) {
            const { organismName, onChange, config } = this.props;
            this.state = {
                organismName,
                onChange,
                config
            }
        } else {
            this.state = {
                organismName: '',
                onChange: null,
                config: {}
            }
        }
    }

    onChange = () => {
        console.log('! ! ! ConfigForm onChange ! ! !');
    }

    render() {
        return (
            <div>
                <SegmentationFormPanel panelName='Segmentation' />
            </div>
        )
    }
}

export default ConfigForm;