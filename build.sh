echo "Uninstalling the old version of ucount"
pip3 uninstall ucount
echo "Done"

echo "Installing new version of ucount"
pip3 install -e .
echo "Done."