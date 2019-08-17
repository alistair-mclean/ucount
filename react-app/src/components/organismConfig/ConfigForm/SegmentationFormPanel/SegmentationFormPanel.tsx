import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';


type FormPanelProps = {
    panelName: string,
    components: null
}

class SegmentationFormPanel extends React.Component<any, FormPanelProps> {
    constructor(props: FormPanelProps) {
        super(props);
        if (props) {
            const { panelName, components } = this.props;
            this.state = {
                panelName,
                components
            }
        } else {
            this.state = {
                panelName: '',
                components: null, 
            }
        }
    }

    render() {
        return (
            <div className='form-panel'>
                <ExpansionPanel>
                    <ExpansionPanelSummary
                    // expandIcon={<ExpandMoreIcon />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    >
                    <Typography className='test'>{this.state.panelName}</Typography>
                </ExpansionPanelSummary>
                    <ExpansionPanelDetails>
                        
                    </ExpansionPanelDetails>
                </ExpansionPanel>
            </div>
        )
    }
}

export default SegmentationFormPanel;