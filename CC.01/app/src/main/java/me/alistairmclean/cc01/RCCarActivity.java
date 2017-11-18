package me.alistairmclean.cc01;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.hardware.Camera;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.provider.MediaStore;
import android.text.format.Time;
import android.util.Log;
import android.view.Display;
import android.view.Surface;
import android.view.SurfaceHolder;
import android.view.View;
import android.view.WindowManager;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.VideoView;
import android.widget.Toast;

import java.io.DataOutputStream;
import java.io.File;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.lang.ref.WeakReference;
import java.net.Socket;
import java.net.UnknownHostException;
import java.security.Policy;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Set;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

//TODO - ADD TCP CONNECTION
public class RCCarActivity extends Activity  implements SurfaceHolder.Callback,
        Camera.PreviewCallback{
    private static final String APP_CLASS = "CC.01" ;
    private static final String CLASS_TAG = "RC_CAR_ACTIVITY";
    private static final String TAG = "Video" ;
    private static int HOST_PORT = 6000;
    private static String HOST_IP = "192.168.1.100";
    public static final String ACTION_USB_PERMISSION = "me.alistairmclean.rc1.USB_PERMISSION";
    private final int VIDEO_REQUEST_CODE = 100;
    //UI elements
    private VideoView mVideoFeed = null;
    private TextView mStatusText = null;
    private Camera mCamera = null;
    private Camera.Size mPreviewSize;
    List<Camera.Size> mSupportedPreviewSizes;
    private SurfaceHolder mHolder = null;
    private boolean isPreviewRunning = false;
    private byte[] pixels = null;
    private boolean mUsbConnected = false;
    private boolean mHostConnected = false;
    private FeatureStreamer fs = new FeatureStreamer();
    private InputStreamReader mInputStreamReader;
    private OutputStreamWriter mOutputStream;
    private static UsbService mUsbService;
    private TextView display;
    private static String mStatusMessage = "Enter in the IP and PORT of the HOST.";
    private EditText mHostIpEditText;
    private EditText mHostPortEditText;
    private MyHandler mHandler;
    private Socket mSocket;

    private final BroadcastReceiver mUsbReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            switch (intent.getAction()) {
                case UsbService.ACTION_USB_PERMISSION_GRANTED: // USB PERMISSION GRANTED
                    Toast.makeText(context, "USB Ready", Toast.LENGTH_SHORT).show();
                    break;
                case UsbService.ACTION_USB_PERMISSION_NOT_GRANTED: // USB PERMISSION NOT GRANTED
                    Toast.makeText(context, "USB Permission not granted", Toast.LENGTH_SHORT).show();
                    break;
                case UsbService.ACTION_NO_USB: // NO USB CONNECTED
                    Toast.makeText(context, "No USB connected", Toast.LENGTH_SHORT).show();
                    break;
                case UsbService.ACTION_USB_DISCONNECTED: // USB DISCONNECTED
                    Toast.makeText(context, "USB disconnected", Toast.LENGTH_SHORT).show();
                    break;
                case UsbService.ACTION_USB_NOT_SUPPORTED: // USB NOT SUPPORTED
                    Toast.makeText(context, "USB device not supported", Toast.LENGTH_SHORT).show();
                    break;
            }
        }
    };


    private final ServiceConnection usbConnection = new ServiceConnection() {
        @Override
        public void onServiceConnected(ComponentName arg0, IBinder arg1) {
            mUsbService = ((UsbService.UsbBinder) arg1).getService();
            mUsbService.setHandler(mHandler);
            mUsbConnected = true;
        }

        @Override
        public void onServiceDisconnected(ComponentName arg0) {
            mUsbService = null;
            mUsbConnected = false;
        }
    };

    public void onIpTextEdit(View view) {
        HOST_IP = mHostIpEditText.getText().toString();
    }
    public void onPortTextEdit(View view) {
        HOST_PORT = Integer.parseInt(mHostPortEditText.getText().toString());
    }


    public String getHostIp() {
        return HOST_IP;
    }

    public int getHostPort() {
        return HOST_PORT;
    }



    public void openSocket(){
        //Create socket connection
        fs.connect(HOST_IP,HOST_PORT);
    }

    public void closeSocket(){
        final String TAG = "CloseSocket";
        //Create socket connection
        try {
            fs.close();
        } catch (IOException e) {
            Log.v(TAG, "IO exception caught when trying to close connection");
        }
    }

    public static byte[] intToByteArray(int value) {
        return new byte[] {
                (byte)(value >>> 24),
                (byte)(value >>> 16),
                (byte)(value >>> 8),
                (byte)value};
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_rccar);

        //Inititialize UI elements
        mStatusText = this.findViewById(R.id.status_text);
        mVideoFeed = (VideoView) findViewById(R.id.video_feed);
        mHostIpEditText = this.findViewById(R.id.host_ip_edit_text);
        mHostPortEditText = this.findViewById(R.id.host_port_edit_text);
        try {
            mCamera = Camera.open();
            mCamera.lock();
            mSupportedPreviewSizes = mCamera.getParameters()
                    .getSupportedPreviewSizes();
        } catch(RuntimeException re) {
            Log.v(TAG, "Could not initialize the Camera");
            re.printStackTrace();
        }

        mVideoFeed = (VideoView) this.findViewById(R.id.video_feed);
        Camera.Size newSize = mCamera.getParameters().getPreviewSize();
        newSize.width = newSize.width/4;
        newSize.height = newSize.height/4;
        mPreviewSize = newSize;
        mCamera.setPreviewCallback(this);
        mHolder = mVideoFeed.getHolder();
        mHolder.addCallback(this);
    }



    public void captureVideo(View view) {
        Intent cameraIntent = new Intent(MediaStore.ACTION_VIDEO_CAPTURE);
        File video_file = getFilePath();
        Uri video_uri = Uri.fromFile(video_file);
        cameraIntent.putExtra(MediaStore.EXTRA_OUTPUT, video_uri);
        cameraIntent.putExtra(MediaStore.EXTRA_VIDEO_QUALITY, 1);
        startActivityForResult(cameraIntent,VIDEO_REQUEST_CODE);
    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if(requestCode == VIDEO_REQUEST_CODE ){
            if(resultCode == RESULT_OK) {
                Toast.makeText(getApplicationContext(),
                        "Video Successfully recorded", Toast.LENGTH_LONG).show();
            }
            else {
                Toast.makeText(getApplicationContext(),
                        "Video Capture Failed or Cancelled", Toast.LENGTH_LONG).show();
            }
        }
    }

    public void setUiEnabled(boolean bool) {
        mStatusText.setEnabled(bool);

    }

    public File getFilePath() {
        File folder = new File("/rc_car_test/");
        if(folder.exists()) {
            folder.mkdir();
        }
        @SuppressLint("SimpleDateFormat") DateFormat dateFormat = new SimpleDateFormat("yyyy_MM_dd__HH_mm_ss");
        Date date = new Date();
        String fileName = dateFormat.format(date) + "_clip.mp4";
        return new File(folder, fileName);
    }


    @Override
    public void surfaceCreated(SurfaceHolder surfaceHolder) {
        final String TAG =  "SURFACECREATED: ";

        try {
            if(mVideoFeed == null)
                mVideoFeed = (VideoView)this.findViewById(R.id.video_feed);
            if(mHolder == null)
                mHolder = mVideoFeed.getHolder();
            mCamera.setPreviewDisplay(mHolder);
            mCamera.setPreviewCallback(this);
            mCamera.startPreview();
            pixels = new byte[mPreviewSize.width * mPreviewSize.height];
            isPreviewRunning = true;
        } catch(IOException e) {
            Log.v(TAG, "IOException. Could not start the preview.");
            e.printStackTrace();
        } catch(NullPointerException e2) {
            Log.e(TAG, "Null pointer exception. Could not start the preview.");
        }
    }

    @Override
    public void surfaceChanged(SurfaceHolder surfaceHolder, int format, int width, int height) {
        try {
            mCamera.stopPreview();
        } catch (Exception e) {
            // ignore: tried to stop a non-existent preview
        }
        if (isPreviewRunning)
        {
            mCamera.stopPreview();
        }

        Camera.Parameters parameters = mCamera.getParameters();
        Display display = ((WindowManager)getSystemService(WINDOW_SERVICE)).getDefaultDisplay();

        if(display.getRotation() == Surface.ROTATION_0)
        {
            parameters.setPreviewSize(height, width);
            mCamera.setDisplayOrientation(90);
        }

        if(display.getRotation() == Surface.ROTATION_90)
        {
            parameters.setPreviewSize(width, height);
        }

        if(display.getRotation() == Surface.ROTATION_180)
        {
            parameters.setPreviewSize(height, width);
        }

        if(display.getRotation() == Surface.ROTATION_270)
        {
            parameters.setPreviewSize(width, height);
            mCamera.setDisplayOrientation(180);
        }

        mCamera.setParameters(parameters);
        previewCamera();
    }


    private void previewCamera() {
        try
        {
            mCamera.setPreviewDisplay(mHolder);
            mCamera.setPreviewCallback(this);
            mCamera.startPreview();
            isPreviewRunning = true;
        }
        catch(Exception e)
        {
            Log.d(APP_CLASS, "Cannot start preview", e);
        }
    }

    @Override
    public void onPreviewFrame(final byte[] data, Camera camera) {
        final String TAG = "ONPREVIEWFRAME: ";
        //Log.v(TAG, "CALLED");

        //If not connected to the host, do nothing.
        if(!mHostConnected)
            return;

        Camera.Parameters parameters = camera.getParameters();
        parameters.set("orientation","portrait");
        camera.setParameters(parameters);
        if (data.length >= mPreviewSize.width * mPreviewSize.height) {

            // NOT SUCCESSFUL vvvvv
            // Attempted to create a new thread to decode to grayscale and send to server
      //      new Thread(new Runnable() {
            //    @Override
        //        public void run() {
          //          synchronized (this) {
                        decodeYUV420SPGrayscale(pixels, data, mPreviewSize.width,
                                mPreviewSize.height);
                        fs.sendFeatures(mPreviewSize.width, mPreviewSize.height, pixels);
                    //}
              //  }
           // });
            Log.d(TAG, "Sending Features to server.");
            mStatusMessage = "Sending Features to server.";
            mStatusText.setText(mStatusMessage);
            Log.d(TAG, "Width = " + mPreviewSize.width + ", Height = " + mPreviewSize.height);

        }

    }

    void close() throws IOException {
        if (mOutputStream != null) {
            mOutputStream.close();
        }
        if (mSocket != null) {
            mSocket.close();
        }
        mHostConnected = false;
    }

    @Override
    public void surfaceDestroyed(SurfaceHolder surfaceHolder) {
        if (mCamera != null) {
            mCamera.stopPreview();
        }
    }

    private Camera.Size getMinimumPreviewSize(List<Camera.Size> sizes, int w, int h) {
        if (sizes == null)
            return null;
        int minWidth = Integer.MAX_VALUE;

        Camera.Size optimalSize = null;
        // Try to find the min size
        for (Camera.Size size : sizes) {
            if (size.width < minWidth) {
                optimalSize = size;
                minWidth = size.width;
            }
        }

        return optimalSize;
    }

    public static void writeToUsb(byte[] val){
        mUsbService.write(val);
    }

    public static void decodeYUV420SPGrayscale(byte[] rgb, byte[] yuv420sp,
                                               int width, int height) {
        final int frameSize = width * height;

        for (int pix = 0; pix < frameSize; pix++) {
            int pixVal = (0xff & ((int) yuv420sp[pix])) - 16;
            if (pixVal < 0)
                pixVal = 0;
            if (pixVal > 255)
                pixVal = 255;
            rgb[pix] = (byte) pixVal;
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        setFilters();  // Start listening notifications from UsbService
        startService(UsbService.class, usbConnection, null); // Start UsbService(if it was not started before) and Bind it
    }

    @Override
    public void onPause() {
        super.onPause();
        unregisterReceiver(mUsbReceiver);
        unbindService(usbConnection);
        if (mCamera != null) {
            mCamera.stopPreview();
            mCamera.release();
            mCamera = null;
        }
    }

    private void startService(Class<?> service, ServiceConnection serviceConnection, Bundle extras) {
        if (!UsbService.SERVICE_CONNECTED) {
            Intent startService = new Intent(this, service);
            if (extras != null && !extras.isEmpty()) {
                Set<String> keys = extras.keySet();
                for (String key : keys) {
                    String extra = extras.getString(key);
                    startService.putExtra(key, extra);
                }
            }
            startService(startService);
        }
        Intent bindingIntent = new Intent(this, service);
        bindService(bindingIntent, serviceConnection, Context.BIND_AUTO_CREATE);
    }

    private void setFilters() {
        IntentFilter filter = new IntentFilter();
        filter.addAction(UsbService.ACTION_USB_PERMISSION_GRANTED);
        filter.addAction(UsbService.ACTION_NO_USB);
        filter.addAction(UsbService.ACTION_USB_DISCONNECTED);
        filter.addAction(UsbService.ACTION_USB_NOT_SUPPORTED);
        filter.addAction(UsbService.ACTION_USB_PERMISSION_NOT_GRANTED);
        registerReceiver(mUsbReceiver, filter);
    }

private static class MyHandler extends Handler {
    private final WeakReference<MainActivity> mActivity;

    public MyHandler(MainActivity activity) {
        mActivity = new WeakReference<>(activity);
    }

    @Override
    public void handleMessage(Message msg) {
        switch (msg.what) {
            case UsbService.MESSAGE_FROM_SERIAL_PORT:
                String data = (String) msg.obj;
                //mActivity.get().display.append(data);
                break;
        }
    }

}
}
